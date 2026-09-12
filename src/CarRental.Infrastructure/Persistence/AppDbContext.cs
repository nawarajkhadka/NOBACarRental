using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Common;
using CarRental.Domain.Entities;
using CarRental.Domain.Exceptions;
using CarRental.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CarCategory> CarCategories => Set<CarCategory>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        StampAuditFields();
        ValidatePendingEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        StampAuditFields();
        ValidatePendingEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // Sets CreatedDate/UpdatedDate on any IAuditable entity being added/modified.
    // CreatedBy/UpdatedBy are set by the caller, since the acting user isn't known here.
    private void StampAuditFields()
    {
        var utcNow = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedDate = utcNow;
            }
        }
    }

    // Runs each pending entity's [Required]/[StringLength]/[Range]/IValidatableObject rules
    // immediately before it reaches the database, regardless of which code path added or
    // modified it.
    private void ValidatePendingEntities()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified))
            {
                continue;
            }

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                entry.Entity,
                new ValidationContext(entry.Entity),
                validationResults,
                validateAllProperties: true);

            if (!isValid)
            {
                throw new DomainValidationException(validationResults[0].ErrorMessage ?? "Validation failed.");
            }
        }
    }
}
