using TariffComparison.Models;
using TariffComparison.Services.TariffCalculators;

namespace TariffComparison.Factory
{
    public interface ITariffCalculatorFactory
    {
        ITariffCalculator GetCalculator(TariffModel tariff);
    }
}
