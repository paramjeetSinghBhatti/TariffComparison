using Microsoft.Extensions.DependencyInjection;
using TariffComparison.Models;

namespace TariffComparison.Services.TariffCalculators
{
    public class BasicTariffCalculator : ITariffCalculator
    {
        private readonly TariffModel _tariff;
        private readonly ILogger<BasicTariffCalculator> _logger;

        public BasicTariffCalculator(TariffModel tariff, ILogger<BasicTariffCalculator> logger)
        {
            _tariff = tariff;
            this._logger = logger;
        }

        public decimal CalculateAnnualCost(int consumption)
        {
            // Try to get the additional cost value from the dictionary
            if (!_tariff.AdditionalProperties.TryGetValue("additionalKwhCost", out var value))
            {
                // Log the error and throw a custom exception to indicate the missing field
                _logger.LogError("The 'additionalKwhCost' field is missing from the tariff properties.");
                throw new KeyNotFoundException("The required field 'additionalKwhCost' is missing from the tariff properties.");
            }

            var baseCost = _tariff.BaseCost * 12;
            var additionalCost = consumption * Convert.ToDecimal(_tariff.AdditionalProperties["additionalKwhCost"]) / 100;
            return baseCost + additionalCost;
        }
    }
}
