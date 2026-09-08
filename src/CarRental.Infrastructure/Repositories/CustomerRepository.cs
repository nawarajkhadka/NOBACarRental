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

    public Task<Customer?> GetBySocialSecurityNumberAsync(string socialSecurityNumber)
        => _dbContext.Customers.FirstOrDefaultAsync(c => c.SocialSecurityNumber == socialSecurityNumber);

    public async Task AddAsync(Customer customer)
        => await _dbContext.Customers.AddAsync(customer);
}
