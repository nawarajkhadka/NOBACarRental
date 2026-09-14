using CarRental.Domain.Entities;

namespace CarRental.Application.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken);
    Task<bool> ExistsByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken);
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
