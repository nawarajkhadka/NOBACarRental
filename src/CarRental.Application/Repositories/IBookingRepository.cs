using CarRental.Domain.Entities;

namespace CarRental.Application.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByBookingNumberAsync(string bookingNumber);
    Task<bool> ExistsByBookingNumberAsync(string bookingNumber);
    Task AddAsync(Booking booking);
    Task SaveChangesAsync();
}
