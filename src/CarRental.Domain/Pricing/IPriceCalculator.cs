namespace CarRental.Domain.Pricing;

public interface IPriceCalculator
{
    string CategoryName { get; }
    decimal Calculate(decimal baseDayRental, decimal baseKmPrice, int numberOfDays, int numberOfKm);
}
