using CarRental.Application.DTOs;

namespace CarRental.Application.Bookings;

public interface IBookingService
{
    Task<BookingResponse> RegisterPickupAsync(RegisterPickupRequest request, int? agentId, CancellationToken cancellationToken);
    Task<BookingResponse> RegisterReturnAsync(RegisterReturnRequest request, int? agentId, CancellationToken cancellationToken);
}
