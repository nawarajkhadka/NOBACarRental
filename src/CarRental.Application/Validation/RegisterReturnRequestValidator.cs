using CarRental.Application.DTOs;
using FluentValidation;

namespace CarRental.Application.Validation;

/// <summary>
/// First-level (request-shape) validation for a return request. Business/data validation
/// (does the booking exist, was it already returned, is the return after pickup, etc.)
/// happens afterwards in BookingService.
/// </summary>
public class RegisterReturnRequestValidator : AbstractValidator<RegisterReturnRequest>
{
    public RegisterReturnRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.BookingNumber)
            .NotEmpty().WithMessage("Booking number is required.")
            .MaximumLength(30).WithMessage("Booking number must not exceed 30 characters.");

        RuleFor(x => x.ReturnDateTime)
            .NotEmpty().WithMessage("Return date/time is required.");

        RuleFor(x => x.ReturnMeterReadingKm)
            .GreaterThanOrEqualTo(0).WithMessage("Return meter reading cannot be negative.");
    }
}
