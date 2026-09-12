# Car Rental System

Small API for registering car pickups and returns, and calculating rental price
based on car category (Small / Combi / Truck).

Stack: .NET 10, EF Core, SQL Server, ASP.NET Core Identity + JWT.

## Running with Docker

1. Copy `.env.example` to `.env` and fill in `SQL_SA_PASSWORD` and `JWT_SIGNING_KEY`.
2. Run:

   ```
   docker compose up --build
   ```

The database schema and seed data are created automatically on first start —
nothing else to run. API is at `http://localhost:8080`, Swagger at
`http://localhost:8080/swagger`.

## Running locally (no Docker)

You need a SQL Server instance reachable from your machine.

1. Update `ConnectionStrings:DefaultConnection` in `src/CarRental.Api/appsettings.json`
   to point at your instance.
2. Run:

   ```
   dotnet run --project src/CarRental.Api
   ```

Same as with Docker, migrations and seed data apply automatically on startup.
Swagger: `https://localhost:7186/swagger` (or the HTTP port if you'd rather skip
trusting the dev cert — `dotnet dev-certs https --trust`).

## Logging in

Two demo accounts are seeded so you can try the API without creating a user
yourself:

- `agent` / `Agent123!`
- `manager` / `Manager123!`

Get a token from `POST /api/auth/login`, then use the **Authorize** button in
Swagger (paste just the token, no `Bearer` prefix) to call the booking endpoints.

## Tests

```
dotnet test
```
