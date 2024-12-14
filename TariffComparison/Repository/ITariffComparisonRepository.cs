using TariffComparison.Models;

namespace TariffComparison.Repository
{
    public interface ITariffComparisonRepository
    {
        Task<IEnumerable<TariffComparisonResult>> CompareTariffsAsync(int consumption);
    }
}
