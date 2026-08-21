# Fulfillment & Inventory Management Platform

Internal platform for managing products, categories, warehouses, and per-warehouse stock. Milestone 1 covers the product and inventory foundation — catalog data, warehouse locations, and stock tracking with a full audit trail.

Full requirements analysis, data model, and review-prep answers live in `Milestone1_Documentation.md`. This file is just setup and run instructions.

## Tech Stack

- .NET 8, ASP.NET Core Web API
- Entity Framework Core + SQL Server
- ASP.NET Identity + JWT bearer authentication
- MediatR (CQRS), FluentValidation, ErrorOr
- Clean Architecture: Domain, Application, Infrastructure, Api, Contracts

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, a full instance, or a container — any of these work)
- EF Core CLI tools:
  ```
  dotnet tool install --global dotnet-ef
  ```

## Setup

1. Clone the repository:
   ```
   git clone <repo-url>
   cd Inventory_Management_Platform
   ```

2. Restore dependencies:
   ```
   dotnet restore
   ```

3. Set your connection string in `Inventory_Management_Platform.Api/appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementPlatform;Trusted_Connection=True;"
   }
   ```
   Swap this for your own SQL Server instance if you're not using LocalDB.

4. Apply migrations to create the database:
   ```
   dotnet ef database update --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api
   ```

5. Run the API:
   ```
   dotnet run --project Inventory_Management_Platform.Api
   ```

6. Open Swagger at the URL printed in the console (typically `https://localhost:7211/swagger`).

On first run in the Development environment, roles and three test accounts are seeded automatically — no manual setup needed to start testing.

## Authenticating in Swagger

1. `POST /api/auth/login` with one of the test accounts below.
2. Copy the `accessToken` from the response.
3. Click **Authorize** at the top of the Swagger page and paste the token in as `Bearer <token>`.
4. All protected endpoints will now include your token automatically.

## Test Users

| Role | Email | Password |
|---|---|---|
| Administrator | admin@example.com | AdminPass123! |
| Warehouse Operator | operator@example.com | OperatorPass123! |
| Manager | manager@example.com | ManagerPass123! |

Additional accounts are created through `POST /api/auth/register` (Administrator-only), which takes a `role` field of `Administrator`, `WarehouseOperator`, or `Manager`.

## Project Structure

```
Inventory_Management_Platform.Domain          — entities, value objects, domain events, business rules
Inventory_Management_Platform.Application     — commands, queries, handlers, validators, repository interfaces
Inventory_Management_Platform.Infrastructure  — EF Core, repositories, Identity, JWT, migrations
Inventory_Management_Platform.Contracts       — request/response DTOs
Inventory_Management_Platform.Api             — controllers, Program.cs, Swagger config
```

## Core Endpoints

| Area | Endpoints |
|---|---|
| Auth | `POST /api/auth/register`, `POST /api/auth/login` |
| Categories | `POST`, `PUT /{id}`, `DELETE /{id}`, `GET /{id}`, `GET` |
| Products | `POST`, `PUT /{id}`, `DELETE /{id}`, `GET /{id}`, `GET` (paged) |
| Warehouses | `POST`, `PUT /{id}`, `DELETE /{id}`, `GET /{id}`, `GET` |
| Stock | `POST /assign`, `POST /adjust`, `GET /warehouse/{id}`, `GET /product/{id}`, `GET /{stockId}/history` |

Write access on Categories/Products/Warehouses is Administrator-only; reads are open to all authenticated roles. Stock assignment is Administrator-only; stock adjustments are Warehouse-Operator-only.

## Known Limitations

- Automated tests aren't included yet.
- The `Register` endpoint's role restriction was temporarily disabled during development for easier testing and needs to be re-enabled (`[Authorize(Roles = "Administrator")]`) before this is treated as submission-ready.
