using CarRental.Domain.Pricing;

namespace CarRental.Application.Pricing;

public interface IPriceCalculatorFactory
{
    IPriceCalculator GetCalculator(string categoryName);
}
