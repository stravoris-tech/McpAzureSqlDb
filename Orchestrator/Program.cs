using System;
using System.Threading.Tasks;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

Console.WriteLine("=== MCP Orchestrator Demo ===");

// Connect to the MCP server via HTTP transport (supports both Streamable HTTP and legacy SSE)
await using var client = await McpClient.CreateAsync(
    new HttpClientTransport(new HttpClientTransportOptions
    {
        Endpoint = new Uri("http://localhost:5017/sse"),
        TransportMode = HttpTransportMode.Sse
    }));

// List available tools
var tools = await client.ListToolsAsync();
Console.WriteLine("\n-- Available Tools --");
foreach (var tool in tools)
{
    Console.WriteLine($"  {tool.Name}: {tool.Description}");
}

// Call get_product_categories
var allCategories = await client.CallToolAsync("get_product_categories");
Console.WriteLine("\n-- All Product Categories --");
foreach (var content in allCategories.Content)
{
    Console.WriteLine(((TextContentBlock)content).Text);
}

// Call get_product_categories_by_parent with parentId = 1 (Bikes)
var bikeCategories = await client.CallToolAsync(
    "get_product_categories_by_parent",
    new Dictionary<string, object?> { { "parentId", 1 } });
Console.WriteLine("\n-- Bike Sub-Categories (parentId=1) --");
foreach (var content in bikeCategories.Content)
{
    Console.WriteLine(((TextContentBlock)content).Text);
}

Console.WriteLine("\nDemo complete. Press any key to exit.");
Console.ReadKey();
