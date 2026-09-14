using CarRental.Domain.Entities;

namespace CarRental.Application.Repositories;

public interface ICarRepository
{
    Task<Car?> GetByRegistrationNumberAsync(string registrationNumber, CancellationToken cancellationToken);
    Task AddAsync(Car car, CancellationToken cancellationToken);
}
