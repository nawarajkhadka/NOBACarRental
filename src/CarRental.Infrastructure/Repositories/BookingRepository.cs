using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _dbContext;

    public BookingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken)
        => _dbContext.Bookings
            .Include(b => b.Car).ThenInclude(c => c.CarCategory)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber, cancellationToken);

    public Task<bool> ExistsByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken)
        => _dbContext.Bookings.AnyAsync(b => b.BookingNumber == bookingNumber, cancellationToken);

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken)
        => await _dbContext.Bookings.AddAsync(booking, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
