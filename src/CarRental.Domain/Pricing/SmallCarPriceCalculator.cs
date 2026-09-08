namespace CarRental.Domain.Pricing;

public class SmallCarPriceCalculator : IPriceCalculator
{
    public string CategoryName => "Small";

    public decimal Calculate(decimal baseDayRental, decimal baseKmPrice, int numberOfDays, int numberOfKm)
        => baseDayRental * numberOfDays;
}
