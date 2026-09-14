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

    public Task<Car?> GetByRegistrationNumberAsync(string registrationNumber, CancellationToken cancellationToken)
        => _dbContext.Cars.FirstOrDefaultAsync(c => c.RegistrationNumber == registrationNumber, cancellationToken);

    public async Task AddAsync(Car car, CancellationToken cancellationToken)
        => await _dbContext.Cars.AddAsync(car, cancellationToken);
}
