using System.Net;
using System.Net.Http.Json;
using CarRental.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace CarRental.Tests.Integration;

public class BookingsAuthorizationTests : IClassFixture<CarRentalApiFactory>
{
    private readonly HttpClient _client;

    public BookingsAuthorizationTests(CarRentalApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterPickup_WithoutToken_ReturnsUnauthorized()
    {
        var request = new RegisterPickupRequest { BookingNumber = "B1" };

        var response = await _client.PostAsJsonAsync("/api/bookings/pickup", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RegisterReturn_WithoutToken_ReturnsUnauthorized()
    {
        var request = new RegisterReturnRequest { BookingNumber = "B1" };

        var response = await _client.PostAsJsonAsync("/api/bookings/return", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
