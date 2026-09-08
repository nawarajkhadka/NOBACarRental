using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class CarRepository : ICarRepository
{
    private readonly AppDbContext _dbContext;

    public CarRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Car?> GetByRegistrationNumberAsync(string registrationNumber)
        => _dbContext.Cars.FirstOrDefaultAsync(c => c.RegistrationNumber == registrationNumber);

    public async Task AddAsync(Car car)
        => await _dbContext.Cars.AddAsync(car);
}
