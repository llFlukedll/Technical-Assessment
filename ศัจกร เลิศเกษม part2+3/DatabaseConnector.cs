using MySqlConnector;
using Microsoft.Extensions.Logging;

public interface IConnectionStringBuilder
{
    string BuildConnectionString();
    int GetCommandTimeout();
}
public class DataBaseConnectionStringBuilder : IConnectionStringBuilder
{
    private readonly DatabaseConfig _config;
    private readonly ILogger<DataBaseConnectionStringBuilder> _logger;

    public DataBaseConnectionStringBuilder(DatabaseConfig config, ILogger<DataBaseConnectionStringBuilder> logger)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string BuildConnectionString()
    {
        var builder = new MySqlConnectionStringBuilder();
        builder.Server = _config.Server;
        builder.Port = _config.Port;
        builder.Database = _config.Database;
        builder.UserID = _config.Username;
        builder.Password = _config.Password;
        builder.Pooling = _config.Pooling;
        builder.MinimumPoolSize = (uint)_config.MinPoolSize;
        builder.MaximumPoolSize = (uint)_config.MaxPoolSize;
        builder.ConnectionTimeout = (uint)_config.ConnectionTimeout;
        _logger.LogInformation("Database connection configured: Server={Server}, Database={DataBase}, Pooling={Pooling}",
        _config.Server, _config.Database, _config.Pooling);

        return builder.ConnectionString;
    }

    public int GetCommandTimeout()
    {
        return _config.CommandTimeout;
    }
}

