# SmartCommerce - Phase 1

Phase 1 creates the first independently deployable microservice: `CatalogService`.

## Why Catalog first?

Catalog owns product descriptive/commercial data. It intentionally does **not** own stock quantity. Stock will be owned by the future `InventoryService`, giving us a genuine service boundary and a real use case for gRPC.

## Current architecture

- .NET 10 / ASP.NET Core
- Clean Architecture style layering
- Dapper
- SQL Server
- Repository + application service
- ProblemDetails / global exception handler
- Swagger
- Central NuGet package management
- GUID identifiers suitable for independently owned services

## Projects

- `CatalogService.Domain` - product domain model
- `CatalogService.Application` - use cases, DTOs, interfaces
- `CatalogService.Infrastructure` - SQL Server/Dapper repository
- `CatalogService.API` - HTTP API

Dependency direction:

`API -> Application <- Infrastructure`

`Application -> Domain`

`Infrastructure -> Domain`

## 1. Prerequisites

Install/use:

- .NET 10 SDK
- Visual Studio 2022 updated to a version supporting .NET 10, or VS Code
- SQL Server / SQL Server Express
- SSMS

Verify:

```powershell
dotnet --version
```

You should see `10.x.x`.

## 2. Create the database

Open SSMS and execute:

`database/001_catalog_database.sql`

This creates:

`SmartCommerce_CatalogDb`

and seeds three demo products.

## 3. Check the connection string

File:

`src/Services/Catalog/CatalogService.API/appsettings.json`

Default:

```text
Server=localhost\SQLEXPRESS;Database=SmartCommerce_CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Change the SQL Server instance only if your machine uses a different instance name.

## 4. Restore and build

From the repository root:

```powershell
dotnet restore SmartCommerce.slnx
dotnet build SmartCommerce.slnx
```

If your Visual Studio/.NET tooling does not support `.slnx`, generate a classic solution:

```powershell
dotnet new sln -n SmartCommerce --format sln
dotnet sln SmartCommerce.sln add .\src\Services\Catalog\CatalogService.Domain\CatalogService.Domain.csproj
dotnet sln SmartCommerce.sln add .\src\Services\Catalog\CatalogService.Application\CatalogService.Application.csproj
dotnet sln SmartCommerce.sln add .\src\Services\Catalog\CatalogService.Infrastructure\CatalogService.Infrastructure.csproj
dotnet sln SmartCommerce.sln add .\src\Services\Catalog\CatalogService.API\CatalogService.API.csproj
dotnet build SmartCommerce.sln
```

## 5. Run CatalogService

```powershell
dotnet run --project .\src\Services\Catalog\CatalogService.API\CatalogService.API.csproj
```

Open:

`https://localhost:7101/swagger`

or the URL printed in the terminal.

## 6. Test endpoints

### GET

`GET /api/products`

### GET by id

`GET /api/products/{productId}`

### POST

```json
{
  "name": "27-inch Monitor",
  "description": "QHD monitor for development",
  "price": 24999
}
```

### PUT

```json
{
  "name": "27-inch QHD Monitor",
  "description": "Updated product description",
  "price": 23999
}
```

### DELETE

`DELETE /api/products/{productId}`

Delete is implemented as a soft delete by setting `IsActive = 0`.

## Interview concepts already present

1. Microservice owns a business capability.
2. Database is owned by its service.
3. Clean dependency direction.
4. Repository hides persistence implementation.
5. Dapper uses parameterized queries.
6. Cancellation tokens flow from HTTP request to SQL command.
7. Product IDs are GUIDs, avoiding dependence on one central identity generator.
8. Global error handling produces consistent HTTP errors.
9. Product catalog deliberately does not own inventory.

## Next phase

Phase 2 adds:

- InventoryService
- separate `SmartCommerce_InventoryDb`
- inventory APIs
- gRPC server in InventoryService
- gRPC client in the next OrderService
- protobuf contracts

We will then add OrderService and use both synchronous gRPC and asynchronous RabbitMQ so the difference is visible in the same project.
