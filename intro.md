# NOBACarRental — Solution Overview

## 1. Problem Statement

The goal was to implement the pickup and return use cases for a car rental system, calculate
rental prices for different car categories, write test cases, and make reasonable assumptions
where the specification was ambiguous.

## 2. Assumptions

- Car and CarCategory are pre-existing reference data (seeded), no create endpoint for either.
- A new Customer is auto-created on pickup if the SSN is not already known.
- Booking number is unique.
- Return date/time must be after pickup date/time.
- Meter reading on return must be greater than or equal to the pickup reading.
- Number of rental days is rounded up (e.g. 25 hours = 2 days).
- Rates are configurable per car category, not hardcoded.
- Both Agent and Manager roles can perform pickup/return; Manager is treated as a superset role.
- Customers never authenticate; authentication is only for staff (Agent/Manager).
- Authentication (JWT) was added although not strictly required, to demonstrate a
  production-style API.

## 3. Technology Choices

- .NET 10 / ASP.NET Core Web API
- EF Core + SQL Server
- ASP.NET Core Identity + JWT
- Clean Architecture (Domain / Application / Infrastructure / Api)
- xUnit + Moq
- Docker


[ARCHITECTURE.md](ARCHITECTURE.md).

## 4. Testing

**Unit Tests**
- Pricing calculators (per category)
- Booking service business logic
- Validation rules

**Integration Tests**
- Controllers (via WebApplicationFactory)
- Authentication/authorization wiring
- EF Core InMemory database

Edge cases covered:
- Return before pickup
- Meter reading decreasing on return
- Unknown/invalid car category
- Duplicate booking number
- Invalid or missing authentication

## 5. Demo

- Login and obtain JWT
- Register a pickup
- Register a return (price calculated)
- View results via Swagger

## 6. Improvements (Future Work)

- CI/CD pipeline
- Refresh tokens / JWT revocation
- Real user registration/admin flow instead of seeded users
- API versioning
- Centralized logging/monitoring
- Rate limiting
- Audit log expansion
