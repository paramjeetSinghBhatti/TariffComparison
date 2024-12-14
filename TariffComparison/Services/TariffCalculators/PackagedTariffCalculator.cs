using TariffComparison.Models;

namespace TariffComparison.Services.TariffCalculators
{
    public class PackagedTariffCalculator : ITariffCalculator
    {
        private readonly TariffModel _tariff;
        private readonly ILogger<PackagedTariffCalculator> _logger;

        public PackagedTariffCalculator(TariffModel tariff, ILogger<PackagedTariffCalculator> logger)
        {
            _tariff = tariff;
            this._logger = logger;
        }

        public decimal CalculateAnnualCost(int consumption)
        {
            // Try to get the additional cost value from the dictionary
            if (!_tariff.AdditionalProperties.TryGetValue("additionalKwhCost", out var additionalKwhCost) 
                || !_tariff.AdditionalProperties.TryGetValue("includedKwh", out var includedKwh))
            {
                // Log the error and throw a custom exception to indicate the missing field
                _logger.LogError("Fields are missing from the tariff properties.");
                throw new KeyNotFoundException("The required fields are missing from the tariff properties.");
            }

            if (consumption <= Convert.ToDecimal(includedKwh))
                return _tariff.BaseCost;

            var additionalConsumption = consumption - Convert.ToDecimal(includedKwh);
            var additionalCost = additionalConsumption * Convert.ToDecimal(additionalKwhCost) / 100;
            return _tariff.BaseCost + additionalCost;
        }
    }
}
