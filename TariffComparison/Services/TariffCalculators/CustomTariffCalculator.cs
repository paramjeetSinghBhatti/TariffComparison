using TariffComparison.Models;

namespace TariffComparison.Services.TariffCalculators
{
    public class CustomTariffCalculator : ITariffCalculator
    {
        private readonly TariffModel _tariff;
        private readonly ILogger<CustomTariffCalculator> logger;

        public CustomTariffCalculator(TariffModel tariff, ILogger<CustomTariffCalculator> logger)
        {
            _tariff = tariff;
            this.logger = logger;
        }

        public TariffModel Tariff { get; }

        public decimal CalculateAnnualCost(int consumption)
        {
            return _tariff.BaseCost + (consumption * 12) / 6;
        }
    }
}
