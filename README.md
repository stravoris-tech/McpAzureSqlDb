# McpAzureSqlDb

**McpAzureSqlDb** is a demonstration project that shows how to build a **Model Context Protocol (MCP)** server in **C#** to connect to a **SQL Database**.
The project uses the **AdventureWorks** sample database and the [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol) NuGet package (v1.0.0).

---

## Project Overview

This repository provides an example of how to:
- Set up an MCP server in a modern .NET solution using the official MCP C# SDK
- Connect the MCP server to a SQL Database (Azure SQL or LocalDB) with AdventureWorks
- Expose safe query methods as MCP tools for product categories and related data
- Build a demo client that connects to the MCP server using the MCP client SDK

---

## Features

- MCP server with built-in tool dispatch via `AddMcpServer()` / `MapMcp()`
- SSE and Streamable HTTP transport support
- Connection management to SQL Database (Azure SQL or LocalDB)
- Example tool (`SqlTool.cs`) exposing two MCP tools:
  - `get_product_categories` — returns all product categories
  - `get_product_categories_by_parent` — filters by parent category ID
- Demo Orchestrator client using the official `McpClient` SDK
- Uses dependency injection for connection string and data access

---

## Project Structure

```
/McpServer                        # ASP.NET Core MCP server
  ├── DatabaseLogic/              # SQL data access helpers (Dapper)
  ├── Models/                     # Data models (ProductCategoryModel)
  ├── Tools/                      # MCP tool classes (SqlTool.cs)
  └── Program.cs                  # Server setup, DI, and MCP endpoint mapping
/Orchestrator                     # Console demo client
  └── Program.cs                  # Connects to MCP server and calls tools
McpAzureSqlDb.sln                 # Solution file
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 or newer (or VS Code with C# extension)
- A SQL Database with the AdventureWorks schema (Azure SQL Database or LocalDB)

---

## Installation & Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/stravoris-tech/McpAzureSqlDb.git
   cd McpAzureSqlDb
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Configure your database connection**

   **Option A — LocalDB (no Azure required):**

   The default `appsettings.json` is pre-configured for LocalDB. You just need to restore the AdventureWorksLT sample database:

   ```bash
   sqllocaldb create MSSQLLocalDB
   sqllocaldb start MSSQLLocalDB
   ```

   Download and restore the [AdventureWorksLT backup](https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorksLT2022.bak) using `sqlcmd` or SSMS.

   **Option B — Azure SQL Database:**

   Update `McpServer/appsettings.json` or use .NET User Secrets:

   ```bash
   dotnet user-secrets set "sqlConnectionInfo:ServerName" "your_server.database.windows.net" --project McpServer
   dotnet user-secrets set "sqlConnectionInfo:InitialCatalog" "AdventureWorksLT" --project McpServer
   dotnet user-secrets set "sqlConnectionInfo:UserId" "your_username" --project McpServer
   dotnet user-secrets set "sqlConnectionInfo:Password" "your_password" --project McpServer
   ```

   And update the `ConnectionStringTemplate` in `appsettings.json` to use SQL authentication:
   ```
   Server={0};Initial Catalog={1};User ID={2};Password={3};TrustServerCertificate=True;
   ```

> **Note for ARM64 machines:** LocalDB ships x64-only native DLLs. You must build and run with the x64 .NET runtime:
> ```bash
> dotnet build McpServer -r win-x64 --self-contained false
> cd McpServer
> "C:\Program Files\dotnet\x64\dotnet" bin/Debug/net8.0/win-x64/McpServer.dll
> ```

---

## Running the MCP Server

```bash
dotnet run --project McpServer
```

The server starts on `http://localhost:5017` with:
- **Health check:** `GET /health`
- **MCP endpoint (Streamable HTTP):** `POST /mcp`
- **MCP endpoint (SSE):** `GET /sse` + `POST /message`

---

## Running the Orchestrator (Demo Client)

With the MCP server running, open a second terminal:

```bash
dotnet run --project Orchestrator
```

Output:

```
=== MCP Orchestrator Demo ===

-- Available Tools --
  get_product_categories: Returns all product categories.
  get_product_categories_by_parent: Returns product categories for a specific parent category.

-- All Product Categories --
[{"parentProductCategoryID":0,"productCategoryID":1,"productCategory":"Bikes"}, ...]

-- Bike Sub-Categories (parentId=1) --
[{"parentProductCategoryID":1,"productCategoryID":5,"productCategory":"Mountain Bikes"}, ...]

Demo complete. Press any key to exit.
```

---

## Example: Using `SqlTool`

The included `SqlTool` demonstrates how to expose safe, server-hosted methods as MCP tools. Methods decorated with `[McpServerTool]` are automatically registered and dispatched by the MCP library:

```csharp
[McpServerToolType]
public class SqlTool
{
    [McpServerTool, Description("Returns all product categories.")]
    public async Task<IEnumerable<ProductCategoryModel>> GetProductCategories() { ... }

    [McpServerTool, Description("Returns product categories for a specific parent category.")]
    public async Task<IEnumerable<ProductCategoryModel>> GetProductCategoriesByParent(
        [Description("The ID of the parent product category.")] int parentId) { ... }
}
```

---

## Learn More

- **Model Context Protocol (MCP):** [MCP Specification](https://modelcontextprotocol.io)
- **MCP C# SDK:** [NuGet - ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol)
- **AdventureWorks Sample DB:** [Microsoft Docs](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure)

---

## Contributing

This is a demonstration project — contributions are welcome for improvements, fixes, or extending the example.
Feel free to fork and open pull requests.

---

## License

This project is licensed under the **MIT License** — see the LICENSE file for details.

---

Enjoy building MCP servers with SQL!
