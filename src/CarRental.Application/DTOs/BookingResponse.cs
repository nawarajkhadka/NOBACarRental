using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs;

public class BookingResponse
{
    public int Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public string CarRegistrationNumber { get; set; } = string.Empty;
    public DateTime PickupDateTime { get; set; }
    public int PickupMeterReadingKm { get; set; }
    public DateTime? ReturnDateTime { get; set; }
    public int? ReturnMeterReadingKm { get; set; }
    public decimal? CalculatedPrice { get; set; }
    public BookingStatus Status { get; set; }
}
