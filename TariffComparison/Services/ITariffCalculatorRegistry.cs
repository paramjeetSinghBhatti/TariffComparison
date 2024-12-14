using TariffComparison.Models;
using TariffComparison.Services.TariffCalculators;

namespace TariffComparison.Services
{
    public interface ITariffCalculatorRegistry
    {
        ITariffCalculator GetCalculator(TariffModel tariff);
    }
}
