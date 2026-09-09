using CarRental.Application.DTOs;
using CarRental.Application.Exceptions;
using CarRental.Application.Pricing;
using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;

namespace CarRental.Application.Bookings;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICarRepository _carRepository;
    private readonly ICarCategoryRepository _carCategoryRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPriceCalculatorFactory _priceCalculatorFactory;

    public BookingService(
        IBookingRepository bookingRepository,
        ICarRepository carRepository,
        ICarCategoryRepository carCategoryRepository,
        ICustomerRepository customerRepository,
        IPriceCalculatorFactory priceCalculatorFactory)
    {
        _bookingRepository = bookingRepository;
        _carRepository = carRepository;
        _carCategoryRepository = carCategoryRepository;
        _customerRepository = customerRepository;
        _priceCalculatorFactory = priceCalculatorFactory;
    }

    public async Task<BookingResponse> RegisterPickupAsync(RegisterPickupRequest request, int? agentId)
    {
        if (await _bookingRepository.ExistsByBookingNumberAsync(request.BookingNumber))
        {
            throw new ApplicationValidationException($"Booking number '{request.BookingNumber}' is already in use.");
        }

        if (await _carCategoryRepository.GetByNameAsync(request.CarCategoryName) is null)
        {
            throw new EntityNotFoundException($"Car category '{request.CarCategoryName}' was not found.");
        }

        var car = await _carRepository.GetByRegistrationNumberAsync(request.RegistrationNumber)
            ?? throw new EntityNotFoundException($"Car with registration number '{request.RegistrationNumber}' is not registered.");

        var customer = await _customerRepository.GetBySocialSecurityNumberAsync(request.CustomerSocialSecurityNumber);
        if (customer is null)
        {
            customer = new Customer
            {
                SocialSecurityNumber = request.CustomerSocialSecurityNumber,
                FirstName = request.CustomerFirstName,
                LastName = request.CustomerLastName,
                CreatedAt = DateTime.UtcNow
            };
            await _customerRepository.AddAsync(customer);
        }

        var booking = new Booking
        {
            BookingNumber = request.BookingNumber,
            Car = car,
            Customer = customer,
            AgentId = agentId,
            PickupDateTime = request.PickupDateTime,
            PickupMeterReadingKm = request.PickupMeterReadingKm,
            Status = BookingStatus.PickedUp
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return ToResponse(booking);
    }

    public async Task<BookingResponse> RegisterReturnAsync(RegisterReturnRequest request)
    {
        var booking = await _bookingRepository.GetByBookingNumberAsync(request.BookingNumber)
            ?? throw new EntityNotFoundException($"Booking '{request.BookingNumber}' was not found.");

        if (booking.Status != BookingStatus.PickedUp)
        {
            throw new ApplicationValidationException($"Booking '{request.BookingNumber}' has already been returned.");
        }

        if (request.ReturnDateTime <= booking.PickupDateTime)
        {
            throw new DomainValidationException("Return date/time must be after the pickup date/time.");
        }

        if (request.ReturnMeterReadingKm < booking.PickupMeterReadingKm)
        {
            throw new DomainValidationException("Return meter reading cannot be less than the pickup meter reading.");
        }

        booking.ReturnDateTime = request.ReturnDateTime;
        booking.ReturnMeterReadingKm = request.ReturnMeterReadingKm;

        var numberOfDays = booking.GetNumberOfDays();
        var numberOfKm = booking.GetNumberOfKm();

        var category = booking.Car.CarCategory;
        var calculator = _priceCalculatorFactory.GetCalculator(category.Name);
        booking.CalculatedPrice = calculator.Calculate(category.BaseDayRental, category.BaseKmPrice, numberOfDays, numberOfKm);
        booking.Status = BookingStatus.Returned;

        await _bookingRepository.SaveChangesAsync();

        return ToResponse(booking);
    }

    private static BookingResponse ToResponse(Booking booking) => new()
    {
        BookingNumber = booking.BookingNumber,
        CarRegistrationNumber = booking.Car.RegistrationNumber,
        PickupDateTime = booking.PickupDateTime,
        PickupMeterReadingKm = booking.PickupMeterReadingKm,
        ReturnDateTime = booking.ReturnDateTime,
        ReturnMeterReadingKm = booking.ReturnMeterReadingKm,
        CalculatedPrice = booking.CalculatedPrice,
        Status = booking.Status
    };
}
