using Clinic.Services;

namespace Clinic.Tests;

public class FixedClock(DateTime now) : IClock
{
    public DateTime Now { get; } = now;
}

public static class TestData
{
    // "Today" in every test is Jan 15, 2026, 8:00 AM.
    public static readonly DateTime Now = new(2026, 1, 15, 8, 0, 0);
    public static FixedClock Clock() => new(Now);
}
