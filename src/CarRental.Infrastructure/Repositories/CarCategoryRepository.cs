using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class CarCategoryRepository : ICarCategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CarCategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CarCategory?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => _dbContext.CarCategories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
}
