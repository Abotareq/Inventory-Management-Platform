# Fulfillment & Inventory Management Platform

A backend API for managing product catalogs, warehouse inventory, and customer orders, built with ASP.NET Core and Clean Architecture.

## Overview

The platform lets a company track products across multiple warehouses, keep stock levels accurate as orders move through their lifecycle, and maintain a full audit trail of every change. It started as a straightforward inventory system (products, categories, warehouses, stock) and grew to cover order processing: creating orders, reserving stock during fulfillment, committing it on completion, and releasing it cleanly on cancellation — without ever double-counting or overselling.

## Features

- Product, category, and warehouse management with full CRUD
- Per-warehouse stock tracking with a two-phase reservation model (physical quantity vs. reserved-for-orders)
- Order lifecycle: Draft → Submitted → Processing → Completed, with cancellation from any pre-completed state
- Commercial value snapshots on order items, so a later price change never rewrites historical order data
- Optimistic concurrency control on Stock and Order, so two simultaneous updates can't silently overwrite each other
- Idempotent order creation via a client-supplied idempotency key, so a retried request doesn't create a duplicate order
- Full audit trail: a generic field-level audit log across every aggregate, plus purpose-built history logs for stock adjustments, stock reservations, and order status transitions
- Role-based access control with four roles: Administrator, Warehouse Operator, Sales Agent, and Manager
- JWT authentication via ASP.NET Identity, with admin-controlled user creation (no public self-registration)
- Global exception handling with correlation IDs, so client-facing errors can be traced back to full server-side logs without exposing internal details
- Paginated, filterable listing endpoints for products, orders, and stock

## Architecture

The solution follows Clean Architecture, split into five projects:

- **Domain** — the core business model: aggregates (Product, Category, Warehouse, Stock, Order, User), value objects, domain events, and business rules. No dependencies on anything else in the solution.
- **Application** — use cases, expressed as MediatR commands and queries with FluentValidation. Defines the repository interfaces the domain needs, without knowing how they're implemented.
- **Infrastructure** — EF Core, the database context, repository implementations, Identity, JWT generation, and the SaveChanges interceptors that dispatch domain events and write audit records.
- **Contracts** — request and response DTOs shared between the API and its consumers.
- **Api** — controllers, middleware, and startup configuration. The only project that knows about HTTP.

Dependencies flow inward: Api depends on Application and Infrastructure, Infrastructure depends on Application and Domain, and Domain depends on nothing.

Business logic that spans multiple steps (order creation, stock adjustments) is handled through domain events. When a Stock or Order aggregate changes state, it raises an event; a `SaveChangesInterceptor` dispatches these events through MediatR before the transaction commits, and dedicated handlers write the corresponding history record (a `StockAdjustment`, `StockReservation`, or `OrderHistory` row) in the same transaction as the state change itself.

## Technologies

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core with SQL Server
- ASP.NET Core Identity for user management, JWT bearer tokens for authentication
- MediatR for CQRS (commands, queries, notifications, and pipeline behaviors)
- FluentValidation for request validation
- ErrorOr for typed, exception-free error handling in the Application layer
- Swashbuckle (Swagger / OpenAPI) with JWT bearer support in the UI

## Project Structure

```
Inventory_Management_Platform.Domain/
├── Category/            Category aggregate
├── Product/              Product aggregate
├── Warehouse/           Warehouse aggregate
├── Stock/                Stock aggregate, StockAdjustment, StockReservation
├── Order/                Order aggregate, OrderItem, OrderHistory
├── User/                 User aggregate and role subtypes (TPT)
├── DomainErrors/        Centralized typed error definitions
└── Common/               Base classes: AggregateRoot, Entity, ValueObject

Inventory_Management_Platform.Application/
├── Categories/           Commands, queries, validators
├── Products/
├── Warehouses/
├── Stocks/               Includes EventHandlers for stock domain events
├── Orders/               Includes EventHandlers for order lifecycle events
├── Authintication/       Register and Login commands
├── AuditLogs/
└── Common/
    ├── Interfaces/       Repository and service interfaces
    ├── Behaviors/        MediatR pipeline behaviors (idempotency)
    ├── Models/           Cross-cutting models (AuditLog, IdempotencyRecord)
    └── Exceptions/       Translated infrastructure exceptions

Inventory_Management_Platform.Infrastructure/
├── Persistence/
│   ├── Configurations/   EF Core entity configurations
│   ├── Repositories/     Repository implementations
│   └── Interceptors/     Domain event dispatch, audit logging
├── Identity/              ApplicationUser, role and test-user seeders
└── Authintication/       JWT token generation, auth service

Inventory_Management_Platform.Contracts/
└── (one folder per feature)  Request and response DTOs

Inventory_Management_Platform.Api/
├── Controllers/
├── Middleware/           Global exception handling, correlation ID logging
└── Program.cs
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB, a full instance, or a container)
- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

### Setup

```bash
git clone <repo-url>
cd Inventory_Management_Platform
dotnet restore
```

Set your connection string in `Inventory_Management_Platform.Api/appsettings.Development.json` (see [Configuration](#configuration) below), then apply migrations and run:

```bash
dotnet ef database update --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api
dotnet run --project Inventory_Management_Platform.Api
```

Swagger opens at the URL printed in the console, typically `https://localhost:7211/swagger`.

Roles and one test account per role are seeded automatically on first run in the Development environment — no manual setup needed to start testing.

## Configuration

`appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  },
  "JwtSettings": {
    "Secret": "YOUR_JWT_SECRET",
    "Issuer": "InventoryManagementApi",
    "Audience": "InventoryManagementClient",
    "ExpiryMinutes": 60
  }
}
```

Replace both placeholders with your own values before running. Don't commit real secrets.

## Database

The project uses Entity Framework Core migrations against SQL Server. Key commands:

```bash
# Add a new migration after changing an entity or configuration
dotnet ef migrations add <MigrationName> --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api

# Apply pending migrations
dotnet ef database update --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api

# Drop the database (useful during development)
dotnet ef database drop --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api
```

Value objects (`ProductId`, `OrderId`, and similar) are mapped through `HasConversion`, so the database stores plain `Guid` columns while the domain works with strongly typed identifiers.

## Authentication & Authorization

Authentication uses ASP.NET Core Identity backed by JWT bearer tokens. There's no public registration endpoint — accounts are created by an Administrator through `POST /api/auth/register`, which takes a `role` field and provisions both the Identity account and the matching domain user record under a shared ID.

Four roles exist, mapped with table-per-type inheritance so each carries only the fields relevant to it:

| Role | Access |
|---|---|
| Administrator | Full CRUD on categories, products, warehouses; creates users; assigns products to warehouses; views audit logs |
| Warehouse Operator | Adjusts stock; processes and completes orders; can cancel orders |
| Sales Agent | Creates and submits orders; can cancel orders |
| Manager | Read access across the system |

Read endpoints generally require only a valid token, regardless of role. Write endpoints check specific roles via `[Authorize(Roles = "...")]`.

Test accounts seeded in Development:

| Role | Email | Password |
|---|---|---|
| Administrator | admin@example.com | AdminPass123! |
| Warehouse Operator | operator@example.com | OperatorPass123! |
| Sales Agent | salesagent@example.com | SalesAgentPass123! |
| Manager | manager@example.com | ManagerPass123! |

To call a protected endpoint from Swagger: log in via `POST /api/auth/login`, copy the returned access token, then click **Authorize** and paste it in as a bearer token.

## API Documentation

Swagger UI is available at the application root when running in Development, with a built-in **Authorize** button for testing JWT-protected endpoints directly from the browser.

## API Endpoints

### Authentication
| Method | Route | Auth | Purpose |
|---|---|---|---|
| POST | `/api/auth/register` | Administrator | Create a user account with a specified role |
| POST | `/api/auth/login` | None | Authenticate and receive a JWT |

### Categories
| Method | Route | Auth | Purpose |
|---|---|---|---|
| POST | `/api/categories` | Administrator | Create a category |
| PUT | `/api/categories/{id}` | Administrator | Rename a category |
| DELETE | `/api/categories/{id}` | Administrator | Delete a category (blocked if any product references it) |
| GET | `/api/categories/{id}` | Any | Get a category by ID |
| GET | `/api/categories` | Any | List all categories |

### Products
| Method | Route | Auth | Purpose |
|---|---|---|---|
| POST | `/api/products` | Administrator | Create a product |
| PUT | `/api/products/{id}` | Administrator | Update a product |
| DELETE | `/api/products/{id}` | Administrator | Delete a product (blocked by existing stock or order references) |
| GET | `/api/products/{id}` | Any | Get a product by ID |
| GET | `/api/products` | Any | List products, paginated |

### Warehouses
| Method | Route | Auth | Purpose |
|---|---|---|---|
| POST | `/api/warehouses` | Administrator | Create a warehouse |
| PUT | `/api/warehouses/{id}` | Administrator | Update a warehouse |
| DELETE | `/api/warehouses/{id}` | Administrator | Delete a warehouse (blocked by existing stock) |
| GET | `/api/warehouses/{id}` | Any | Get a warehouse by ID |
| GET | `/api/warehouses` | Any | List all warehouses |

### Stock
| Method | Route | Auth | Purpose |
|---|---|---|---|
| POST | `/api/stocks/assign` | Administrator | Assign a product to a warehouse, starting at zero quantity |
| POST | `/api/stocks/adjust` | Warehouse Operator | Increase or decrease physical stock, with a required reason |
| DELETE | `/api/stocks/{id}` | Administrator | Delete a stock record (blocked by active reservations, remaining quantity, or order history) |
| GET | `/api/stocks/warehouse/{warehouseId}` | Any | List stock for a warehouse, paginated |
| GET | `/api/stocks/product/{productId}` | Any | List stock for a product across warehouses, paginated |
| GET | `/api/stocks/{stockId}/history` | Any | Adjustment history for a stock record |
| GET | `/api/stocks/{stockId}/reservations` | Any | Reservation history (reserved / released / committed) for a stock record |

Example adjust request:
```json
{
  "productId": "b67f1e80-b95b-4c34-bd3b-99a0acb78c5b",
  "warehouseId": "8880a05b-175a-441b-84ff-2e9dbd65eee9",
  "amount": 50,
  "reason": "Initial delivery"
}
```

### Orders
| Method | Route | Auth | Purpose |
|---|---|---|---|
| POST | `/api/orders` | Sales Agent | Create a draft order with line items; idempotent via an `idempotencyKey` |
| POST | `/api/orders/{id}/submit` | Sales Agent | Move an order from Draft to Submitted |
| POST | `/api/orders/{id}/begin-processing` | Warehouse Operator | Move to Processing and reserve stock for each line item |
| POST | `/api/orders/{id}/complete` | Warehouse Operator | Complete the order and commit reserved stock |
| POST | `/api/orders/{id}/cancel` | Sales Agent, Warehouse Operator | Cancel the order; releases any reserved stock |
| GET | `/api/orders/{id}` | Any | Get an order by ID |
| GET | `/api/orders` | Any | List orders, paginated, filterable by customer, status, and date range |
| GET | `/api/orders/{id}/history` | Any | Status transition history for an order |

Example create request:
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    {
      "productId": "b67f1e80-b95b-4c34-bd3b-99a0acb78c5b",
      "warehouseId": "8880a05b-175a-441b-84ff-2e9dbd65eee9",
      "quantity": 5
    }
  ],
  "idempotencyKey": "a-unique-client-generated-string"
}
```

### Audit
| Method | Route | Auth | Purpose |
|---|---|---|---|
| GET | `/api/audit-logs` | Administrator | List field-level change records across all entities, filterable by entity name and ID |

## Error Handling & Validation

Request validation runs through FluentValidation as a MediatR pipeline step, before a request reaches its handler. Validation failures return a `400` with per-field error messages.

Business rule violations inside a handler (a duplicate SKU, an invalid order status transition, insufficient stock) are modeled with ErrorOr rather than exceptions, and mapped to the appropriate HTTP status by a shared `Problem()` helper on the base controller — `404` for not found, `409` for conflicts, `400` for validation.

Two categories of infrastructure failure are caught and translated at the database layer: SQL unique-constraint violations become a typed `UniqueConstraintViolationException`, and EF Core concurrency conflicts (from optimistic concurrency tokens on Stock and Order) become a `ConcurrencyConflictException`. Handlers that write to these tables catch both and return a clean, typed error instead of letting a raw database exception reach the client.

Any exception that isn't otherwise handled is caught by a global exception middleware, logged in full server-side (including a correlation ID that matches the `traceId` returned to the client), and surfaced to the caller as a generic error with no internal details exposed.

## Development

```bash
# Build
dotnet build

# Run
dotnet run --project Inventory_Management_Platform.Api

# Add a migration
dotnet ef migrations add <Name> --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api

# Apply migrations
dotnet ef database update --project Inventory_Management_Platform.Infrastructure --startup-project Inventory_Management_Platform.Api
```

## Future Improvements

These are reasonable next steps, not currently implemented:

- Automated unit and integration tests, particularly around Stock's invariants and the order lifecycle's status transitions
- Customer as a full aggregate, rather than a bare reference ID on Order
- Extending idempotency support beyond order creation to the other order-lifecycle commands
- A lighter-weight order summary response for the paginated list endpoint, to avoid loading full line-item detail for every row
- Product activation and deactivation as an alternative to hard deletion
- Structured log output to an external sink (Serilog, Application Insights, or similar) rather than console-only logging
