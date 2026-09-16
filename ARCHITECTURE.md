# Architecture

## High-level overview

```mermaid
flowchart TB

    subgraph Client
        Agent[Rental Agent]
    end

    subgraph API["CarRental.Api"]
        Controllers[Controllers]
        Auth[JWT Authentication]
    end

    subgraph Application["CarRental.Application"]
        BookingService[Booking Service]
        PricingFactory[Pricing Factory]
    end

    subgraph Domain["CarRental.Domain"]
        Entities[Entities]
        Rules[Business Rules]
        Pricing[IPriceCalculator]
    end

    subgraph Infrastructure["CarRental.Infrastructure"]
        Repositories[Repositories]
        Identity[ASP.NET Identity]
        Database[(SQL Server)]
    end

    Agent --> Controllers
    Controllers --> Auth
    Controllers --> BookingService

    BookingService --> PricingFactory
    BookingService --> Repositories

    PricingFactory --> Pricing

    Repositories --> Database
    Identity --> Database
```

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
