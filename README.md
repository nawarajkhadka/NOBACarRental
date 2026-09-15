# Car Rental System

Small API for registering car pickups and returns, and calculating rental price
based on car category (Small / Combi / Truck).

Stack: .NET 10, EF Core, SQL Server, ASP.NET Core Identity + JWT.

## Running with Docker

1. Create a `.env` file in the repo root with:

   ```
   SQL_SA_PASSWORD='STRONGpw1!'
   JWT_SIGNING_KEY=CHANGE_ME_TO_A_LONG_RANDOM_SECRET_AT_LEAST_32_BYTES
   ```

   (`SQL_SA_PASSWORD` must satisfy SQL Server's complexity policy — at least
   8 characters, with characters from at least 3 of: uppercase, lowercase,
   digits, symbols.)

2. Run:

   ```
   docker compose up --build
   ```

The database schema and seed data are created automatically on first start —
nothing else to run. API is at `http://localhost:8080`, Swagger at
`http://localhost:8080/swagger`.

To stop and remove the containers:

```
docker compose down
```

Add `-v` to also delete the SQL Server data volume (e.g. if you change
`SQL_SA_PASSWORD` and need a clean re-seed).

## Logging in

Two demo accounts are seeded so you can try the API without creating a user
yourself:

- `agent` / `Agent123!`
- `manager` / `Manager123!`

Get a token from `POST /api/auth/login`, then use the **Authorize** button in
Swagger (paste just the token, no `Bearer` prefix) to call the booking endpoints.

Example:

```
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"agent","password":"Agent123!"}'
```

## Tests

Requires the .NET 10 SDK (tests run outside Docker):

```
dotnet test
```
