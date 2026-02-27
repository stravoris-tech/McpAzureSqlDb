# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

McpAzureSqlDb is a C# MCP (Model Context Protocol) server that exposes Azure SQL Database queries (AdventureWorks sample DB) as tools via JSON-RPC 2.0. It includes a demo console client (Orchestrator) that exercises the server endpoints.

## Build & Run Commands

```bash
# Build the full solution
dotnet build

# Run the MCP server (http://localhost:5017)
dotnet run --project McpServer

# Run the demo client (requires server running)
dotnet run --project Orchestrator
```

There are no tests in this project currently.

## Configuration

Database credentials are in `McpServer/appsettings.json` under `sqlConnectionInfo`. For local development, use .NET User Secrets (UserSecretsId: `778961f8-0321-40a0-b12a-2c746bceca91`):

```bash
dotnet user-secrets set "sqlConnectionInfo:ServerName" "your_server" --project McpServer
```

## Architecture

**Two projects in the solution:**

- **McpServer** — ASP.NET Core 8.0 web app exposing a JSON-RPC endpoint at `POST /mcp` and a health check at `GET /health`. Manual JSON-RPC dispatch in `Program.cs` routes method calls to tool classes.
- **Orchestrator** — Console app that sends JSON-RPC requests to the MCP server via `HttpClient`.

**McpServer internal structure:**

- `Program.cs` — DI setup (all services registered as singletons), endpoint definitions, and JSON-RPC request routing. Defines `JsonRpcRequest`/`JsonRpcResponse` records.
- `Tools/SqlTool.cs` — MCP tool class (decorated with `[McpServerToolType]`) with methods `GetProductCategories()` and `GetProductCategoriesByParent(int parentId)`. Each method runs a SQL query via `SqlDataAccess`.
- `DatabaseLogic/SqlDataAccess.cs` — Generic data access layer using Dapper. Single method `LoadData<T, U>()` executes parameterized SQL and returns typed results.
- `DatabaseLogic/ConnectionStringService.cs` — Builds connection strings from config values (`IConnectionStringService` interface).
- `Models/ProductCategoryModel.cs` — Maps to `SalesLT.ProductCategory` table.

**Key dependencies:** Dapper (ORM), Microsoft.Data.SqlClient, ModelContextProtocol 0.3.0-preview.1 (preview — API may change).
