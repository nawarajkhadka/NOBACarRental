namespace CarRental.Domain.Pricing;

public class CombiPriceCalculator : IPriceCalculator
{
    public string CategoryName => "Combi";

    public decimal Calculate(decimal baseDayRental, decimal baseKmPrice, int numberOfDays, int numberOfKm)
        => baseDayRental * numberOfDays * 1.3m + baseKmPrice * numberOfKm;
}
