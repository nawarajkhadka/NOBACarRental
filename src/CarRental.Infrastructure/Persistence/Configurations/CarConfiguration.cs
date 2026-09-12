using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Persistence.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Car");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.RegistrationNumber).IsRequired().HasMaxLength(20);

        builder.HasIndex(c => c.RegistrationNumber).IsUnique();
        builder.HasIndex(c => c.CarCategoryId);

        builder.HasOne(c => c.CarCategory)
            .WithMany(cc => cc.Cars)
            .HasForeignKey(c => c.CarCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Demo car per category — no API exists to create these otherwise.
        builder.HasData(
            new Car { Id = 1, RegistrationNumber = "ABC123", CarCategoryId = 1 },
            new Car { Id = 2, RegistrationNumber = "DEF456", CarCategoryId = 2 },
            new Car { Id = 3, RegistrationNumber = "GHI789", CarCategoryId = 3 });
    }
}
