using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarRental.Tests.Integration;

/// <summary>Boots the API with an in-memory database so no real MSSQL instance is required.</summary>
public class CarRentalApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("UseInMemoryDatabase", "true");
        builder.UseSetting("InMemoryDatabaseName", $"CarRentalTests-{Guid.NewGuid()}");
    }
}
