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
  Non-clustered index on every FK (Car.CarCategoryId, Booking.CarId/CustomerId/AgentId).

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

## Commands
- Build: `dotnet build` (from repo root). Test: `dotnet test`.
- Docker: `docker compose up --build` (requires `.env` from `.env.example`: `SQL_SA_PASSWORD`,
  `JWT_SIGNING_KEY`).

