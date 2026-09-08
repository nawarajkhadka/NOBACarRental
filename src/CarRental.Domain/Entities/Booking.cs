using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;

namespace CarRental.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // Staff member who registered the pickup/return; nullable per spec.
    public int? AgentId { get; set; }

    public DateTime PickupDateTime { get; set; }
    public int PickupMeterReadingKm { get; set; }

    public DateTime? ReturnDateTime { get; set; }
    public int? ReturnMeterReadingKm { get; set; }
    public decimal? CalculatedPrice { get; set; }

    public BookingStatus Status { get; set; }

    // Whole days between pickup and return, rounded UP (25 hours counts as 2 days).
    public int GetNumberOfDays()
    {
        if (ReturnDateTime is null)
        {
            throw new DomainValidationException("Cannot calculate number of days before the booking is returned.");
        }

        var duration = ReturnDateTime.Value - PickupDateTime;
        return (int)Math.Ceiling(duration.TotalDays);
    }

    public int GetNumberOfKm()
    {
        if (ReturnMeterReadingKm is null)
        {
            throw new DomainValidationException("Cannot calculate number of km before the booking is returned.");
        }

        var numberOfKm = ReturnMeterReadingKm.Value - PickupMeterReadingKm;
        if (numberOfKm < 0)
        {
            throw new DomainValidationException("Return meter reading cannot be less than the pickup meter reading.");
        }

        return numberOfKm;
    }
}
