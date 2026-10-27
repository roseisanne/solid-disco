using Clinic.Services;

namespace Clinic.Tests;

// TASK 5: The first group passes today. Keep them green while you refactor.
public class CopayCalculatorTests
{
    private readonly CopayCalculator _calc = new();

    [Theory]
    [InlineData("HMO", 100, false, 20)]
    [InlineData("HMO", 100, true, 40)]
    [InlineData("PPO", 100, false, 30)]
    [InlineData("PPO", 100, true, 50)]
    [InlineData("HDHP", 80, false, 80)]
    [InlineData("Medicare", 120, false, 12)]
    public void Calculates_ExpectedCopay(string plan, int cost, bool specialist, int expected) =>
        Assert.Equal((decimal)expected, _calc.Calculate(plan, cost, specialist));

    [Fact]
    public void CopayNeverExceedsVisitCost() =>
        Assert.Equal(15m, _calc.Calculate("HMO", 15m, false));

    [Fact]
    public void UnknownPlan_Throws() =>
        Assert.Throws<ArgumentException>(() => _calc.Calculate("Banana", 100m, false));

    // New "story" that arrives AFTER your refactor: Medicaid has a flat $5 copay.
    // This fails until you add it. It should be a small change if your refactor is good.
    [Fact]
    public void Medicaid_IsFlatFiveDollars() =>
        Assert.Equal(5m, _calc.Calculate("Medicaid", 100m, false));
}
