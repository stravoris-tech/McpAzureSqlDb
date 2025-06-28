namespace McpServer.DatabaseLogic;


public class ConnectionStringService : IConnectionStringService
{
    private readonly string _serverName;
    private readonly string _initialCatalog;
    private readonly string _userId;
    private readonly string _password;
    private readonly string _template;

    public ConnectionStringService(IConfiguration config)
    {
        _serverName = config["sqlConnectionInfo:ServerName"]!;
        _initialCatalog = config["sqlConnectionInfo:InitialCatalog"]!;
        _userId = config["sqlConnectionInfo:UserId"]!;
        _password = config["sqlConnectionInfo:Password"]!;
        _template = config["sqlConnectionInfo:ConnectionStringTemplate"]!;
    }

    /// <summary>
    /// Builds the connection string on the fly.
    /// </summary>
    public string ConnectionString
        => string.Format(_template, _serverName, _initialCatalog, _userId, _password);

    /// <summary>
    /// Interface implementation; returns the same thing, but
    /// as an async-friendly Task for DI consumers.
    /// </summary>
    public Task<string> GetConnectionString()
        => Task.FromResult(ConnectionString);
}
