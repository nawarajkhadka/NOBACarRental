namespace CarRental.Domain.Entities;

public class CarCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal BaseDayRental { get; set; }
    public decimal BaseKmPrice { get; set; }

    public ICollection<Car> Cars { get; set; } = new List<Car>();
}
