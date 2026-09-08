using CarRental.Domain.Entities;

namespace CarRental.Application.Repositories;

public interface ICarCategoryRepository
{
    Task<CarCategory?> GetByNameAsync(string name);
}
