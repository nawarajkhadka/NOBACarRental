using CarRental.Domain.Entities;

namespace CarRental.Application.Repositories;

public interface ICarRepository
{
    Task<Car?> GetByRegistrationNumberAsync(string registrationNumber);
    Task AddAsync(Car car);
}
