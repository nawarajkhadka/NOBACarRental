using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Customer?> GetBySocialSecurityNumberAsync(string socialSecurityNumber, CancellationToken cancellationToken)
        => _dbContext.Customers.FirstOrDefaultAsync(c => c.SocialSecurityNumber == socialSecurityNumber, cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
        => await _dbContext.Customers.AddAsync(customer, cancellationToken);
}
