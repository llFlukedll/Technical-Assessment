
using Dapper;
using System.Data;
using MySqlConnector;
using Microsoft.Extensions.Logging;
public class CustomerService
{
    private readonly IConnectionStringBuilder _connectionStringBuilder;
    private readonly ILogger<CustomerService> _logger;
    private readonly int _commandTimeout;

    public CustomerService(
        IConnectionStringBuilder connectionStringBuilder,
        ILogger<CustomerService> logger)
    {
        _connectionStringBuilder = connectionStringBuilder ?? throw new ArgumentNullException(nameof(connectionStringBuilder));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandTimeout = _connectionStringBuilder.GetCommandTimeout();
    }

    private IDbConnection CreateConnection()
    {
        var connectionString = _connectionStringBuilder.BuildConnectionString();
        return new MySqlConnection(connectionString);
    }

    public async Task<Customer?> GetCustomerInfoAsync(string id
    //for part 3 add
    //,DateTime? startdate = null, DateTime? endDate=null
    )
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Customer ID cannot be null or empty", nameof(id));
        _logger.LogInformation("Retrieving customer with ID : {CustomerId}", id);
        try
        {
            using var connection = CreateConnection();
            //for part 3 add
            // var (sql, parameters) = (startDate.HasValue, endDate.HasValue) switch
            // {
            //     (true, true) => (
            //         "SELECT * FROM Customers WHERE Id = @Id AND CreatedDate >= @StartDate AND CreatedDate <= @EndDate",
            //         (object)new { Id = id, StartDate = startDate.Value, EndDate = endDate.Value }
            //     ),

            //     (true, false) => (
            //         "SELECT * FROM Customers WHERE Id = @Id AND CreatedDate >= @StartDate",
            //         (object)new { Id = id, StartDate = startDate.Value }
            //     ),

            //     (false, true) => (
            //         "SELECT * FROM Customers WHERE Id = @Id AND CreatedDate <= @EndDate",
            //         (object)new { Id = id, EndDate = endDate.Value }
            //     ),

            //     (false, false) => (
            //         "SELECT * FROM Customers WHERE Id = @Id",
            //         (object)new { Id = id }
            //     )

            // };
            //for part 3 commet the line below
            const string sql = "SELECT * FROM Customers WHERE Id = @Id";
            var customer = await connection.QuerySingleOrDefaultAsync<Customer>(
                sql, 
                new { Id = id }, //chage this line to parameters for part 3
                commandTimeout: _commandTimeout  
            );
            _logger.LogInformation("Customer retrieval {Status} for ID: {CustomerId}",
            customer != null ? "successful" : "not found", id);
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with ID: {CustomerId}", id);
            throw new InvalidOperationException($"Error retrieving customer with ID: {id}", ex);
        }
    }

}