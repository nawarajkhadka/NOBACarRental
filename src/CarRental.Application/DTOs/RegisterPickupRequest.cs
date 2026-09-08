namespace CarRental.Application.DTOs;

public class RegisterPickupRequest
{
    public string BookingNumber { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string CustomerSocialSecurityNumber { get; set; } = string.Empty;
    public string CustomerFirstName { get; set; } = string.Empty;
    public string CustomerLastName { get; set; } = string.Empty;
    public string CarCategoryName { get; set; } = string.Empty;
    public DateTime PickupDateTime { get; set; }
    public int PickupMeterReadingKm { get; set; }
}
