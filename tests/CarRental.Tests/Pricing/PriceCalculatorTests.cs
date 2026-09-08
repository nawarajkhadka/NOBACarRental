using CarRental.Domain.Pricing;
using FluentAssertions;
using Xunit;

namespace CarRental.Tests.Pricing;

public class PriceCalculatorTests
{
    [Fact]
    public void SmallCarPriceCalculator_ChargesBaseDayRentalOnly()
    {
        var calculator = new SmallCarPriceCalculator();

        var price = calculator.Calculate(baseDayRental: 300m, baseKmPrice: 2m, numberOfDays: 3, numberOfKm: 500);

        price.Should().Be(900m); // 300 * 3
    }

    [Fact]
    public void CombiPriceCalculator_AppliesThirtyPercentDaySurchargePlusKmPrice()
    {
        var calculator = new CombiPriceCalculator();

        var price = calculator.Calculate(baseDayRental: 400m, baseKmPrice: 3m, numberOfDays: 2, numberOfKm: 100);

        price.Should().Be(1340m); // 400 * 2 * 1.3 + 3 * 100
    }

    [Fact]
    public void TruckPriceCalculator_AppliesFiftyPercentSurchargeOnDayAndKmPrice()
    {
        var calculator = new TruckPriceCalculator();

        var price = calculator.Calculate(baseDayRental: 500m, baseKmPrice: 4m, numberOfDays: 1, numberOfKm: 200);

        price.Should().Be(1950m); // 500 * 1 * 1.5 + 4 * 200 * 1.5
    }
}
