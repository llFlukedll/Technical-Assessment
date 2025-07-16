
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)  
            .Build();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var databaseConfig = configuration.GetSection("Database").Get<DatabaseConfig>() ?? new DatabaseConfig();
                services.AddSingleton(databaseConfig);

                services.AddScoped<IConnectionStringBuilder, DataBaseConnectionStringBuilder>();
                services.AddScoped<CustomerService>();

                services.AddLogging(builder => builder.AddConsole());
            })
            .Build();

        await TestDatabaseConnection(host);
    }

    private static async Task TestDatabaseConnection(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var customerService = scope.ServiceProvider.GetRequiredService<CustomerService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            logger.LogInformation("Testing database connection...");
            var customer = await customerService.GetCustomerInfoAsync("1");
            logger.LogInformation("Database connection test completed successfully");
            logger.LogInformation("Customer details:\n{CustomerDetails}", customer.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database connection test failed");
        }
    }
}