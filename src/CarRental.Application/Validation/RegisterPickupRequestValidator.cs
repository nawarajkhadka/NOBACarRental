using CarRental.Application.DTOs;
using FluentValidation;

namespace CarRental.Application.Validation;

/// <summary>
/// First-level (request-shape) validation for a pickup request — required fields,
/// lengths, obviously-invalid values. Business/data validation (does the booking
/// number already exist, is the car registered, etc.) happens afterwards in BookingService.
/// </summary>
public class RegisterPickupRequestValidator : AbstractValidator<RegisterPickupRequest>
{
    public RegisterPickupRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.BookingNumber)
            .NotEmpty().WithMessage("Booking number is required.")
            .MaximumLength(30).WithMessage("Booking number must not exceed 30 characters.");

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty().WithMessage("Registration number is required.")
            .MaximumLength(20).WithMessage("Registration number must not exceed 20 characters.");

        RuleFor(x => x.CustomerSocialSecurityNumber)
            .NotEmpty().WithMessage("Customer social security number is required.")
            .MaximumLength(20).WithMessage("Customer social security number must not exceed 20 characters.");

        RuleFor(x => x.CustomerFirstName)
            .NotEmpty().WithMessage("Customer first name is required.")
            .MaximumLength(100).WithMessage("Customer first name must not exceed 100 characters.");

        RuleFor(x => x.CustomerLastName)
            .NotEmpty().WithMessage("Customer last name is required.")
            .MaximumLength(100).WithMessage("Customer last name must not exceed 100 characters.");

        RuleFor(x => x.CarCategoryName)
            .NotEmpty().WithMessage("Car category name is required.")
            .MaximumLength(50).WithMessage("Car category name must not exceed 50 characters.");

        RuleFor(x => x.PickupDateTime)
            .NotEmpty().WithMessage("Pickup date/time is required.");

        RuleFor(x => x.PickupMeterReadingKm)
            .GreaterThanOrEqualTo(0).WithMessage("Pickup meter reading cannot be negative.");
    }
}
