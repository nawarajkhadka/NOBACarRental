# Architecture

Clean Architecture, four projects, dependencies only point inward.



- Domain has no dependencies at all. entities, enums, and the pricing strategies.
- Application defines the use cases (IBookingService) and the interfaces
  Infrastructure has to implement (IBookingRepository, ...).
- Infrastructure implements those interfaces: AppDbContext, repositories,
  ASP.NET Core Identity, JWT token generation.
- Api is the composition root, wires the concrete implementations into
  the DI container in Program.cs.

## Request flow — register pickup

```mermaid
sequenceDiagram
    participant Client
    participant Controller as BookingsController
    participant Service as BookingService
    participant Repo as Repositories
    participant DB as AppDbContext / SQL Server

    Client->>Controller: POST /api/bookings/pickup (JWT)
    Controller->>Service: RegisterPickupAsync(request, agentId)
    Service->>Repo: check booking number, get car/category, get/create customer
    Repo->>DB: queries
    Service->>DB: add Booking (CreatedBy = agentId)
    DB-->>Service: saved
    Service-->>Controller: BookingResponse
    Controller-->>Client: 200 OK
```

Return follows the same shape, but also resolves the right IPriceCalculator
for the car's category.

## Request flow — register return

```mermaid
sequenceDiagram
    participant Client
    participant Controller as BookingsController
    participant Service as BookingService
    participant Repo as BookingRepository
    participant Calc as IPriceCalculatorFactory
    participant DB as AppDbContext / SQL Server

    Client->>Controller: POST /api/bookings/return (JWT)
    Controller->>Service: RegisterReturnAsync(request, agentId)
    Service->>Repo: GetByBookingNumberAsync
    Repo->>DB: query
    DB-->>Service: Booking (Car, CarCategory, Customer)
    Service->>Service: check not already returned, return date/km valid
    Service->>Calc: GetCalculator(category.Name)
    Calc-->>Service: IPriceCalculator
    Service->>Service: Calculate(price), set Status = Returned, UpdatedBy = agentId
    Service->>DB: SaveChangesAsync
    DB-->>Service: saved
    Service-->>Controller: BookingResponse
    Controller-->>Client: 200 OK
```

## Persistence

EF Core migrations apply automatically on startup (<b>Database.Migrate()</b>` in program.cs, so there's no manual database setup step. Seed data creates the
three car categories and one demo car per category, no api to create.

## Auth

ASP.NET Core Identity handles users/roles/password hashing; login returns a
JWT with the user's roles baked in as claims. [Authorize(Roles = "Agent,Manager")]
protects the booking endpoints
