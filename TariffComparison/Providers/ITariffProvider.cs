using TariffComparison.Models;

namespace TariffComparison.Providers
{
    public interface ITariffProvider
    {
        Task<IEnumerable<TariffModel>> GetTariffsAsync();
    }
}
