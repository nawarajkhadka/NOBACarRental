using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;

namespace CarRental.Domain.Entities;

public class Booking : IValidatableObject
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(30)]
    public string BookingNumber { get; set; } = string.Empty;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // Staff member who registered the pickup/return; nullable per spec.
    public int? AgentId { get; set; }

    public DateTime PickupDateTime { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Pickup meter reading cannot be negative.")]
    public int PickupMeterReadingKm { get; set; }

    public DateTime? ReturnDateTime { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Return meter reading cannot be negative.")]
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

    // Cross-field rules that a single property attribute can't express.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ReturnDateTime is not null && ReturnDateTime <= PickupDateTime)
        {
            yield return new ValidationResult(
                "Return date/time must be after the pickup date/time.",
                new[] { nameof(ReturnDateTime) });
        }

        if (ReturnMeterReadingKm is not null && ReturnMeterReadingKm < PickupMeterReadingKm)
        {
            yield return new ValidationResult(
                "Return meter reading cannot be less than the pickup meter reading.",
                new[] { nameof(ReturnMeterReadingKm) });
        }
    }
}
