namespace Clinic.Services;

public class CopayCalculator
{
    // TASK 5: this works and its tests pass, but it's hard to read and hard to extend.
    // Refactor it (keep tests green), then add the new "Medicaid" plan.
    public decimal Calculate(string planType, decimal visitCost, bool isSpecialist)
    {
        decimal copay;
        if (planType == "HMO")
        {
            if (isSpecialist) { copay = 40m; } else { copay = 20m; }
        }
        else if (planType == "PPO")
        {
            if (isSpecialist) { copay = 50m; } else { copay = 30m; }
        }
        else if (planType == "HDHP")
        {
            copay = visitCost;
        }
        else if (planType == "Medicare")
        {
            copay = Math.Round(visitCost * 0.10m, 2);
        }
        else
        {
            throw new ArgumentException($"Unknown plan type: {planType}");
        }

        if (copay > visitCost) { copay = visitCost; }
        return copay;
    }
}
