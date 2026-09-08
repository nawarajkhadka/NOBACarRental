using CarRental.Domain.Entities;
using CarRental.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace CarRental.Tests.Domain;

public class BookingCalculationTests
{
    [Theory]
    [InlineData(24, 1)]   // exactly 1 day
    [InlineData(25, 2)]   // 1 hour over a full day rounds up to 2
    [InlineData(48, 2)]   // exactly 2 days
    [InlineData(49, 3)]   // 1 hour over 2 days rounds up to 3
    public void GetNumberOfDays_RoundsUpToWholeDays(int rentalHours, int expectedDays)
    {
        var pickup = new DateTime(2026, 1, 1, 9, 0, 0);
        var booking = new Booking
        {
            PickupDateTime = pickup,
            ReturnDateTime = pickup.AddHours(rentalHours)
        };

        booking.GetNumberOfDays().Should().Be(expectedDays);
    }

    [Fact]
    public void GetNumberOfKm_ReturnsDifferenceBetweenMeterReadings()
    {
        var booking = new Booking
        {
            PickupMeterReadingKm = 1000,
            ReturnMeterReadingKm = 1250
        };

        booking.GetNumberOfKm().Should().Be(250);
    }

    [Fact]
    public void GetNumberOfKm_ThrowsWhenReturnReadingIsLessThanPickupReading()
    {
        var booking = new Booking
        {
            PickupMeterReadingKm = 1000,
            ReturnMeterReadingKm = 900
        };

        var act = () => booking.GetNumberOfKm();

        act.Should().Throw<DomainValidationException>();
    }
}
