# Copilot Instructions — Car Rental System

.NET 10 Web API, Clean Architecture, EF Core + MSSQL, ASP.NET Core Identity + JWT for staff.
Two use cases: RegisterPickup, RegisterReturn (triggers price calculation). Keep it simple —
no MediatR/CQRS.

## Solution layout
- `src/CarRental.Domain` — entities, enums, `IPriceCalculator` + 3 strategies. No dependencies.
- `src/CarRental.Application` — DTOs, repo interfaces, `IBookingService`/`BookingService`,
  `IPriceCalculatorFactory`, `ITokenService` (takes `IAuthenticatedUser`, not `ApplicationUser`
  directly — keeps Application from depending on Infrastructure's Identity types).
- `src/CarRental.Infrastructure` — `AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>`,
  Fluent API configs (`Persistence/Configurations`), repo impls, `TokenService`, `RoleSeeder`
  (hosted service — resolves `RoleManager` via `IServiceScopeFactory`, never inject scoped
  services directly into a singleton `IHostedService`), `AddInfrastructure(services, config, useInMemoryDatabase=false)`.
- `src/CarRental.Api` — controllers (`AuthController`, `BookingsController`), `Program.cs`.
  Has `public partial class Program {}` at end for `WebApplicationFactory<Program>` tests.
- `tests/CarRental.Tests` — xUnit + FluentAssertions + Moq; `CarRentalApiFactory` (integration,
  in-memory DB via `UseInMemoryDatabase`/`InMemoryDatabaseName` config settings).

Dependency direction: `Api -> Infrastructure -> Application -> Domain`.

## Domain rules
- Entities: `Customer` (SSN unique, PII — never log/expose), `CarCategory` (Name unique),
  `Car` (RegistrationNumber unique), `Booking` (BookingNumber unique, `Status` enum
  PickedUp/Returned, `GetNumberOfDays()` rounds UP hours→days, `GetNumberOfKm()` throws
  `DomainValidationException` if negative).
- Pricing (`CarRental.Domain.Pricing`): `SmallCarPriceCalculator` = day*rate;
  `CombiPriceCalculator` = day*rate*1.3 + km*rate; `TruckPriceCalculator` = day*rate*1.5 + km*rate*1.5.
  `PriceCalculatorFactory` (Application) resolves by `CarCategory.Name` — add new category =
  new calculator class + DI registration only.
- Unique indexes: Customer.SSN, CarCategory.Name, Car.RegistrationNumber, Booking.BookingNumber.
  Non-clustered index on every FK (Car.CarCategoryId, Booking.CarId/CustomerId/CreatedBy/UpdatedBy).
- Booking implements `IAuditable` (`CarRental.Domain.Common`): `CreatedDate`/`UpdatedDate` are
  auto-stamped by `AppDbContext.StampAuditFields()` on save; `CreatedBy`/`UpdatedBy` (nullable FK
  to AspNetUsers) are set explicitly by `BookingService` from the acting agent id — `CreatedBy`
  at pickup, `UpdatedBy` at return. There used to be a separate `AgentId` for "which agent
  handled this," but it was always set to the same value as `CreatedBy` and never read
  independently, so it was removed in favor of the audit columns. Only Booking has audit
  columns — Customer/CarCategory/Car have no update path, so they'd go unused.

## Auth
- Roles: `Agent`, `Manager` (seeded by `RoleSeeder`). `BookingsController` requires
  `[Authorize(Roles = "Agent,Manager")]`. Customers never log in.
- JWT/connection string come from config (`Jwt:*`, `ConnectionStrings:DefaultConnection`) —
  never hard-code. Local dev values are placeholders in `appsettings.json`; override via
  env vars / `.env` (see `docker-compose.yml`, `.env.example`).

## Known library quirks (see /memories/dotnet-swagger-openapi2.md for full notes)
- Swashbuckle.AspNetCore 10.x / Microsoft.OpenApi 2.x: namespace is `Microsoft.OpenApi` (no
  `.Models`); use `new OpenApiSecuritySchemeReference("Bearer")` instead of
  `OpenApiSecurityScheme.Reference`; `AddSecurityRequirement` takes
  `Func<OpenApiDocument, OpenApiSecurityRequirement>`.

## Database / migrations
- EF Core migrations live in `src/CarRental.Infrastructure/Persistence/Migrations`. `Program.cs`
  calls `dbContext.Database.Migrate()` on startup (skipped when `useInMemoryDatabase` is true),
  so the schema is created/updated automatically against the `sql` container — no manual
  `dotnet ef database update` needed to run the app.
- `CarCategoryConfiguration`/`CarConfiguration` seed 3 categories (`Small`/`Combi`/`Truck`,
  matching `IPriceCalculator.CategoryName`) and one demo car per category via `HasData`, since
  there's no API to create categories/cars.
- After changing an entity or `IEntityTypeConfiguration`, add a new migration:
  `dotnet ef migrations add <Name> --project src/CarRental.Infrastructure --startup-project src/CarRental.Api --output-dir Persistence/Migrations`.

## Commands
- Build: `dotnet build` (from repo root). Test: `dotnet test`.
- Docker: `docker compose up --build` (requires `.env` from `.env.example`: `SQL_SA_PASSWORD`,
  `JWT_SIGNING_KEY`). First run creates the DB schema and seed data automatically.

