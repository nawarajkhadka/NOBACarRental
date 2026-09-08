using CarRental.Application.Bookings;
using CarRental.Application.DTOs;
using CarRental.Application.Exceptions;
using CarRental.Application.Pricing;
using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Pricing;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarRental.Tests.Bookings;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepository = new();
    private readonly Mock<ICarRepository> _carRepository = new();
    private readonly Mock<ICarCategoryRepository> _carCategoryRepository = new();
    private readonly Mock<ICustomerRepository> _customerRepository = new();
    private readonly Mock<IPriceCalculatorFactory> _priceCalculatorFactory = new();
    private readonly BookingService _sut;

    public BookingServiceTests()
    {
        _sut = new BookingService(
            _bookingRepository.Object,
            _carRepository.Object,
            _carCategoryRepository.Object,
            _customerRepository.Object,
            _priceCalculatorFactory.Object);
    }

    [Fact]
    public async Task RegisterPickupAsync_ThrowsWhenBookingNumberAlreadyInUse()
    {
        _bookingRepository.Setup(r => r.ExistsByBookingNumberAsync("B1")).ReturnsAsync(true);

        var request = new RegisterPickupRequest { BookingNumber = "B1" };

        var act = () => _sut.RegisterPickupAsync(request, agentId: 1);

        await act.Should().ThrowAsync<ApplicationValidationException>();
    }

    [Fact]
    public async Task RegisterReturnAsync_ThrowsWhenBookingAlreadyReturned()
    {
        var booking = new Booking
        {
            BookingNumber = "B1",
            Status = BookingStatus.Returned,
            PickupDateTime = DateTime.UtcNow.AddDays(-1),
            PickupMeterReadingKm = 100
        };
        _bookingRepository.Setup(r => r.GetByBookingNumberAsync("B1")).ReturnsAsync(booking);

        var request = new RegisterReturnRequest { BookingNumber = "B1", ReturnDateTime = DateTime.UtcNow, ReturnMeterReadingKm = 200 };

        var act = () => _sut.RegisterReturnAsync(request);

        await act.Should().ThrowAsync<ApplicationValidationException>();
    }

    [Fact]
    public async Task RegisterReturnAsync_ThrowsWhenReturnMeterReadingIsLessThanPickup()
    {
        var booking = new Booking
        {
            BookingNumber = "B1",
            Status = BookingStatus.PickedUp,
            PickupDateTime = DateTime.UtcNow.AddDays(-1),
            PickupMeterReadingKm = 500
        };
        _bookingRepository.Setup(r => r.GetByBookingNumberAsync("B1")).ReturnsAsync(booking);

        var request = new RegisterReturnRequest { BookingNumber = "B1", ReturnDateTime = DateTime.UtcNow, ReturnMeterReadingKm = 100 };

        var act = () => _sut.RegisterReturnAsync(request);

        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task RegisterReturnAsync_CalculatesPriceUsingCategorySpecificCalculator()
    {
        var category = new CarCategory { Name = "Small", BaseDayRental = 300m, BaseKmPrice = 2m };
        var car = new Car { RegistrationNumber = "ABC123", CarCategory = category };
        var booking = new Booking
        {
            BookingNumber = "B1",
            Status = BookingStatus.PickedUp,
            Car = car,
            Customer = new Customer(),
            PickupDateTime = new DateTime(2026, 1, 1),
            PickupMeterReadingKm = 1000
        };
        _bookingRepository.Setup(r => r.GetByBookingNumberAsync("B1")).ReturnsAsync(booking);

        var calculator = new Mock<IPriceCalculator>();
        calculator.Setup(c => c.Calculate(300m, 2m, 2, 100)).Returns(600m);
        _priceCalculatorFactory.Setup(f => f.GetCalculator("Small")).Returns(calculator.Object);

        var request = new RegisterReturnRequest
        {
            BookingNumber = "B1",
            ReturnDateTime = new DateTime(2026, 1, 2, 1, 0, 0), // 25 hours -> rounds up to 2 days
            ReturnMeterReadingKm = 1100
        };

        var response = await _sut.RegisterReturnAsync(request);

        response.CalculatedPrice.Should().Be(600m);
        response.Status.Should().Be(BookingStatus.Returned);
    }
}
