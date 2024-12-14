using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using TariffComparison.Models;
using TariffComparison.Services.TariffCalculators;

namespace TariffComparison.Services
{
    /// <summary>
    /// This class takes care of handling any new implementation of Product Types. We can register the new Type and it's 
    /// Calculator implemetation class name in appsettings.json file which is then read here and resolved using reflection.
    /// This minimizes the number of changes for new types.
    /// </summary>
    public class TariffCalculatorRegistry : ITariffCalculatorRegistry
    {
        private readonly IDictionary<int, Func<TariffModel, ITariffCalculator>> _calculatorRegistryMapping;
        private readonly ILogger<TariffCalculatorRegistry> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TariffCalculatorRegistry(IConfiguration configuration,
                                        ILogger<TariffCalculatorRegistry> logger,
                                        IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _calculatorRegistryMapping = Initialize(configuration);
        }

        /// <summary>
        /// we initialize the tariff calculator dictionary here using reflection, DI.
        /// The configuration we have setup in the appsettings.development.json
        /// get the config section from the app settings file and convert it into a dictionary of int and string
        /// which represents Key= Tariff Type and Value = Tariff Calculation implementation.
        /// here we create and return a new dictionary where the key is same but the valus is a function which returns us the
        /// ITariff inmplemetation type based on the Tariff Type key we have passed.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private IDictionary<int, Func<TariffModel, ITariffCalculator>> Initialize(IConfiguration configuration)
        {
            var calculatorMappings = configuration.GetSection("TariffCalculators")?.Get<Dictionary<int, string>>();

            if (calculatorMappings == null || !calculatorMappings.Any())
            {
                _logger.LogError("No tariff calculator mappings found in the configuration.");
                return null;
            }

            return calculatorMappings.ToDictionary(
                kvp => kvp.Key,
                kvp => CreateCalculatorInstance(kvp.Value)
            );
        }

        /// <summary>
        /// Return the function that helps us to create the instance of ITariff based on Type.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Func<TariffModel, ITariffCalculator> CreateCalculatorInstance(string className)
        {
            var type = Type.GetType(className);
            if (type == null)
            {
                _logger.LogError("Class type not found for: {ClassName}", className);
                throw new InvalidOperationException($"Class type not found: {className}");
            }

            return tariff =>
            {
                try
                {
                    // Get the ILogger<T> for the specific calculator type to pass to the ITariff types.
                    var logger = _serviceProvider.GetRequiredService(typeof(ILogger<>).MakeGenericType(type));

                    // create instance of ITariffCalculator using reflection.
                    var instance = (ITariffCalculator)Activator.CreateInstance(type, new[] { tariff, logger });
                    return instance;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to resolve instance of {ClassName}", className);
                    throw;
                }
            };
        }

        /// <summary>
        /// This method gets called from repo and executes the func to return the instance
        /// of ITariffCalculator type based on mapping.
        /// </summary>
        /// <param name="tariff"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public ITariffCalculator GetCalculator(TariffModel tariff)
        {
            if (_calculatorRegistryMapping == null || _calculatorRegistryMapping.Count == 0)
            {
                _logger.LogError("No items found in the Registry mapping.");
                return null;
            }

            if (_calculatorRegistryMapping.TryGetValue(tariff.Type, out var createCalculator))
                return createCalculator(tariff);

            return null;
            //_logger.LogError("No calculator registered for tariff type {TariffType}", tariff.Type);
            //throw new KeyNotFoundException($"No calculator registered for tariff type {tariff.Type}");
        }
    }
}