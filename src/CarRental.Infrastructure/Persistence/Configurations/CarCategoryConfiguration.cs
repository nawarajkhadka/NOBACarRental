using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Persistence.Configurations;

public class CarCategoryConfiguration : IEntityTypeConfiguration<CarCategory>
{
    public void Configure(EntityTypeBuilder<CarCategory> builder)
    {
        builder.ToTable("CarCategory");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.BaseDayRental).HasColumnType("decimal(10,2)");
        builder.Property(c => c.BaseKmPrice).HasColumnType("decimal(10,2)");

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
