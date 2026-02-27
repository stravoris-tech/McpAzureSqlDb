using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.AspNetCore;
using McpServer.DatabaseLogic;
using McpServer.Tools;

var builder = WebApplication.CreateBuilder(args);

// DI registrations
builder.Services
    .AddSingleton<IConnectionStringService, ConnectionStringService>()
    .AddSingleton<SqlDataAccess>()
    .AddMcpServer()
      .WithHttpTransport()
      .WithTools<SqlTool>();

var app = builder.Build();

// Health check
app.MapGet("/health", () => Results.Ok("alive"));

// MCP JSON-RPC endpoint (handles capabilities + tool dispatch automatically)
app.MapMcp();

app.Run();
