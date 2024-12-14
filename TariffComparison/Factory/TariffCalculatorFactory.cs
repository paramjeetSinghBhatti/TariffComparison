using TariffComparison.Models;
using TariffComparison.Services;
using TariffComparison.Services.TariffCalculators;

namespace TariffComparison.Factory
{
    public class TariffCalculatorFactory : ITariffCalculatorFactory
    {
        private readonly ITariffCalculatorRegistry _calculatorRegistry;

        public TariffCalculatorFactory(ITariffCalculatorRegistry calculatorRegistry)
        {
            _calculatorRegistry = calculatorRegistry;
        }

        public ITariffCalculator GetCalculator(TariffModel tariff)
        {
            return _calculatorRegistry.GetCalculator(tariff);
        }
    }
}
