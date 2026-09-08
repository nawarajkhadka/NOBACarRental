namespace CarRental.Domain.Pricing;

public class TruckPriceCalculator : IPriceCalculator
{
    public string CategoryName => "Truck";

    public decimal Calculate(decimal baseDayRental, decimal baseKmPrice, int numberOfDays, int numberOfKm)
        => baseDayRental * numberOfDays * 1.5m + baseKmPrice * numberOfKm * 1.5m;
}
