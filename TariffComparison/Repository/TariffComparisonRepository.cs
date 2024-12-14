
using TariffComparison.Factory;
using TariffComparison.Models;
using TariffComparison.Providers;

namespace TariffComparison.Repository
{
    public class TariffComparisonRepository : ITariffComparisonRepository
    {
        private readonly ITariffProvider _provider;
        private readonly ITariffCalculatorFactory _calculatorFactory;
        private readonly ILogger<TariffComparisonRepository> _logger;

        public TariffComparisonRepository(ITariffProvider provider, ITariffCalculatorFactory calculatorFactory,
            ILogger<TariffComparisonRepository> logger)
        {
            _provider = provider;
            _calculatorFactory = calculatorFactory;
            this._logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumption"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TariffComparisonResult>> CompareTariffsAsync(int consumption)
        {
            //Get the tariffs from the provider. This is a mock method returning us Tariffs reading
            //from a json file.
            var tariffs = await _provider.GetTariffsAsync();
            if (tariffs == null || !tariffs.Any())
            {
                _logger.LogError("Tariffs returned from tariff provider were null or empty.");
                return new List<TariffComparisonResult>();
            }

            var comparisonResults = new List<TariffComparisonResult>();
            
            foreach (var tariff in tariffs)
            {
                if(tariff == null)
                    continue;

                var tariffComparisonResult = new TariffComparisonResult();
                // we get the type of Tariff Calculator based on the type of Tariff supplied here.
                //Caluclator factory used the Tariff Registry class to create instance of ITariff implementations.
                var calculator = _calculatorFactory.GetCalculator(tariff);

                //For some reason, if we failt to create instance of ITariff type, we skip over it and continue the 
                //execution with other tariffs. Choice was to fail the complete transaction but I chose to do the below.
                if(calculator==null)
                    continue;

                tariffComparisonResult.TariffName = tariff.Name;
                tariffComparisonResult.AnnualCost = calculator.CalculateAnnualCost(consumption);
                comparisonResults.Add(tariffComparisonResult);
            }

            return comparisonResults.OrderBy(x => x.AnnualCost);
        }
    }
}
