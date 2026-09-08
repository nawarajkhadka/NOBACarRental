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

    public Task<Booking?> GetByBookingNumberAsync(string bookingNumber)
        => _dbContext.Bookings
            .Include(b => b.Car).ThenInclude(c => c.CarCategory)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber);

    public Task<bool> ExistsByBookingNumberAsync(string bookingNumber)
        => _dbContext.Bookings.AnyAsync(b => b.BookingNumber == bookingNumber);

    public async Task AddAsync(Booking booking)
        => await _dbContext.Bookings.AddAsync(booking);

    public Task SaveChangesAsync()
        => _dbContext.SaveChangesAsync();
}
