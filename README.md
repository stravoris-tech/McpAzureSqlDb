# McpAzureSqlDb

**McpAzureSqlDb** is a demonstration project that shows how to build a **Model Context Protocol (MCP)** server in **C#** to connect to an **Azure SQL Database**.  
The project uses the **AdventureWorks** sample database and leverages a preview NuGet package for the MCP implementation.

---

## Project Overview

This repository provides an example of how to:
- Set up an MCP server in a modern .NET solution
- Connect the MCP server to a live Azure SQL Database (AdventureWorks)
- Expose safe query methods for product categories and related data

---

## Features

- MCP server implementation in C#
- Connection management to Azure SQL Database
- Example tool (`SqlTool.cs`) that exposes safe database methods
- Uses dependency injection for connection string and data access
- Simple demo entry point (`Program.cs`)

---

## Project Structure

```
/McpServer
  ├── Controllers/             # (If applicable: API controllers)
  ├── DatabaseLogic/           # SQL data access helpers
  ├── Models/                  # Data models for MCP and database
  ├── Tools/                   # MCP tool classes (e.g., SqlTool.cs)
  ├── Program.cs               # Main entry point
  ├── McpAzureSqlDb.sln        # Solution file
  ├── README.md                # This file
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 or newer (or VS Code with C# extension)
- An Azure SQL Database instance with the AdventureWorks schema

> **Note:** The MCP NuGet package used in this project is **preview** and may be subject to change.

---

## Installation & Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/KrishnaGMohan/McpAzureSqlDb.git
   cd McpAzureSqlDb
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Update connection string**

   - Locate your connection string settings (likely in `appsettings.json` or via environment variables).
   - Point it to your Azure SQL Database (AdventureWorks).

---

## Running the MCP Server

Run the project from the solution root:

```bash
dotnet run --project McpServer
```

You should see output similar to:

```
=== MCP Orchestrator Demo ===
-- Capabilities --
...
Demo complete. Press any key to exit.
```

---

## Example: Using `SqlTool`

The included `SqlTool` demonstrates how to expose safe, server-hosted methods to query product categories from the AdventureWorks database using MCP.

```csharp
[McpServerToolType]
public class SqlTool {
    // Example: query product categories safely
}
```

---

## Learn More

- **Model Context Protocol (MCP):** [Microsoft MCP Repo](https://github.com/microsoft/ModelContextProtocol)
- **AdventureWorks Sample DB:** [Microsoft Docs](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure)

---

## Contributing

This is a demonstration project — contributions are welcome for improvements, fixes, or extending the example.  
Feel free to fork and open pull requests.

---

## License

This project is licensed under the **MIT License** — see the LICENSE file for details.

---

## Disclaimer

> This project uses a **preview** MCP NuGet package for demonstration purposes only. Use at your own discretion for production projects.

---

Enjoy building MCP servers with Azure SQL!
