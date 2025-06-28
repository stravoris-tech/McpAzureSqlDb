using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

Console.WriteLine("=== MCP Orchestrator Demo ===");

var mcpUrl = "http://localhost:5017/mcp";

using var client = new HttpClient();

var capsRequest = new { jsonrpc = "2.0", method = "getCapabilities", id = 1 };
var capsResponse = await client.PostAsJsonAsync(mcpUrl, capsRequest);
string capsJson = await capsResponse.Content.ReadAsStringAsync();
Console.WriteLine("\n-- Capabilities --");
Console.WriteLine(capsJson);

var callRequest = new
{
    jsonrpc = "2.0",
    method = "SqlTool.GetProductCategories",
    @params = new { },
    id = 2
};
var callResponse = await client.PostAsJsonAsync(mcpUrl, callRequest);
string callJson = await callResponse.Content.ReadAsStringAsync();
Console.WriteLine("\n-- Tool Call Result --");
Console.WriteLine(callJson);

// In a real LLM scenario, you would parse 'callJson' and feed the data into the model prompt.

Console.WriteLine("\nDemo complete. Press any key to exit.");
Console.ReadKey();