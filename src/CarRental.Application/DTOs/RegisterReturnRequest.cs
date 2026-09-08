namespace CarRental.Application.DTOs;

public class RegisterReturnRequest
{
    public string BookingNumber { get; set; } = string.Empty;
    public DateTime ReturnDateTime { get; set; }
    public int ReturnMeterReadingKm { get; set; }
}
