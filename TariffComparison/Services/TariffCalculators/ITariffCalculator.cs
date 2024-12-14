namespace TariffComparison.Services.TariffCalculators
{
    public interface ITariffCalculator
    {
        decimal CalculateAnnualCost(int consumption);
    }
}
