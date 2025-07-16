public class DatabaseConfig
{
    public string Server { get; set; }
    public uint Port { get; set; } = 3306;
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Pooling { get; set; } = true;
    public int MinPoolSize { get; set; } = 5;
    public int MaxPoolSize { get; set; } = 100;
    public int ConnectionTimeout { get; set; } = 30;
    public int CommandTimeout { get; set; } = 30;
}
// Root configuration class
public class AppSettings
{
    public DatabaseConfig Database { get; set; } = new();
}
