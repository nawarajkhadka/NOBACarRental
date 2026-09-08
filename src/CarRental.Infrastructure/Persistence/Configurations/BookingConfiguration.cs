using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Booking");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookingNumber).IsRequired().HasMaxLength(30);
        builder.Property(b => b.PickupDateTime).IsRequired();
        builder.Property(b => b.PickupMeterReadingKm).IsRequired();
        builder.Property(b => b.CalculatedPrice).HasColumnType("decimal(10,2)");
        builder.Property(b => b.Status).HasConversion<byte>();

        builder.HasIndex(b => b.BookingNumber).IsUnique();
        builder.HasIndex(b => b.CarId);
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.AgentId);

        builder.HasOne(b => b.Car)
            .WithMany()
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Customer)
            .WithMany()
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(b => b.AgentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
