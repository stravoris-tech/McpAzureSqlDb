namespace McpServer.DatabaseLogic;

public interface IConnectionStringService
{
    string ConnectionString { get; }
    Task<string> GetConnectionString();
}