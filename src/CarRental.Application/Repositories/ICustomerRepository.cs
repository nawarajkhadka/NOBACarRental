using CarRental.Domain.Entities;

namespace CarRental.Application.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetBySocialSecurityNumberAsync(string socialSecurityNumber);
    Task AddAsync(Customer customer);
}
