using System.Text.Json;
using McpServer.DatabaseLogic;
using McpServer.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// DI registrations
builder.Services.AddSingleton<IConnectionStringService, ConnectionStringService>();
builder.Services.AddSingleton<SqlDataAccess>();
builder.Services.AddSingleton<SqlTool>();

var app = builder.Build();

// Health check
app.MapGet("/health", () => Results.Ok("alive"));

// JSON-RPC endpoint
app.MapPost("/mcp", async (HttpContext http, SqlTool tool) =>
{
    var rpc = await http.Request.ReadFromJsonAsync<JsonRpcRequest>();
    if (rpc == null || rpc.Jsonrpc != "2.0")
        return Results.BadRequest();

    object? result = null;
    object? error = null;

    switch (rpc.Method)
    {
        case "getCapabilities":
            // build capabilities with consistent parameter types
            var capabilities = new
            {
                tools = new[]
                {
                    new
                    {
                        name = "SqlTool",
                        methods = new[]
                        {
                            new
                            {
                                name = "GetProductCategories",
                                description = "Returns all product categories",
                                parameters = new Dictionary<string, string>()
                            },
                            new
                            {
                                name = "GetProductCategoriesByParent",
                                description = "Returns product categories filtered by parent id",
                                parameters = new Dictionary<string, string> { { "parentId", "integer" } }
                            }
                        }
                    }
                }
            };
            result = capabilities;
            break;

        case "SqlTool.GetProductCategories":
            result = await tool.GetProductCategories();
            break;

        case "SqlTool.GetProductCategoriesByParent":
            var p = rpc.Params.GetProperty("parentId").GetInt32();
            result = await tool.GetProductCategoriesByParent(p);
            break;

        default:
            error = new { code = -32601, message = "Method not found" };
            break;
    }

    var response = new JsonRpcResponse
    {
        Jsonrpc = "2.0",
        Result = error == null ? result! : null,
        Error = error,
        Id = rpc.Id
    };

    return Results.Json(response);
});

app.Run();

// POCOs for JSON-RPC
public record JsonRpcRequest(string Jsonrpc, string Method, JsonElement Params, int Id);
public record JsonRpcResponse
{
    public string Jsonrpc { get; set; } = "2.0";
    public object Result { get; set; }
    public object? Error { get; set; }
    public int Id { get; set; }
}
// =======================================================================================================================
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using ModelContextProtocol.AspNetCore;
//using McpServer.DatabaseLogic;
//using McpServer.Tools;

//var builder = WebApplication.CreateBuilder(args);

//// Register your DI dependencies
//builder.Services
//    .AddSingleton<IConnectionStringService, ConnectionStringService>()
//    .AddSingleton<SqlDataAccess>()
//    .AddSingleton<SqlTool>()
//    .AddMcpServer()
//      .WithHttpTransport()
//      .WithTools<SqlTool>();


//var app = builder.Build();

//// optional health-check endpoint
//app.MapGet("/health", () => Results.Ok("alive"));

//// 3. map the MCP JSON-RPC endpoint (incl. getCapabilities + method dispatch)
//app.MapMcp();

//app.Run();

//using McpServer.Tools;
//using ModelContextProtocol.Protocol;
//using System.Text.Json;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Register SqlTool with DI:
//builder.Services.AddSingleton<SqlTool>();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//// JSON-RPC endpoint:
//app.MapPost("/mcp", async (SqlTool tool, HttpContext http) =>
//{
//    // Basic JSON-RPC dispatcher
//    var rpcRequest = await http.Request.ReadFromJsonAsync<JsonRpcRequest>();

//    if (rpcRequest.Method == "SqlTool.GetProductCategories")
//    {
//        var result = await tool.GetProductCategories();

//        return Results.Json(new JsonRpcResponse
//        {
//            JsonRpc = "2.0",
//            Result = result,
//            Id = rpcRequest.Id
//        });
//    }

//    return Results.Json(new JsonRpcResponse
//    {
//        JsonRpc = "2.0",
//        Error = new JsonRpcError
//        {
//            Code = -32601,
//            Message = "Method not found"
//        },
//        Id = rpcRequest.Id
//    });
//});

//app.Run();

//record JsonRpcRequest(string Jsonrpc, string Method, JsonElement Params, int Id);
//record JsonRpcResponse
//{
//    public string JsonRpc { get; set; }
//    public object Result { get; set; }
//    public JsonRpcError Error { get; set; }
//    public int Id { get; set; }
//}
//record JsonRpcError 
//{ 
//    public int Code { get; set; } 
//    public string Message { get; set; } 
//}