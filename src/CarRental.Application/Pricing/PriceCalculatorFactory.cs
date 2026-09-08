using CarRental.Domain.Pricing;

namespace CarRental.Application.Pricing;

public class PriceCalculatorFactory : IPriceCalculatorFactory
{
    private readonly Dictionary<string, IPriceCalculator> _calculatorsByCategory;

    public PriceCalculatorFactory(IEnumerable<IPriceCalculator> calculators)
    {
        _calculatorsByCategory = calculators.ToDictionary(
            c => c.CategoryName,
            StringComparer.OrdinalIgnoreCase);
    }

    public IPriceCalculator GetCalculator(string categoryName)
    {
        if (!_calculatorsByCategory.TryGetValue(categoryName, out var calculator))
        {
            throw new InvalidOperationException($"No price calculator registered for category '{categoryName}'.");
        }

        return calculator;
    }
}
