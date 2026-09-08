namespace CarRental.Domain.Entities;

public class Car
{
    public int Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public int CarCategoryId { get; set; }
    public CarCategory CarCategory { get; set; } = null!;
}
