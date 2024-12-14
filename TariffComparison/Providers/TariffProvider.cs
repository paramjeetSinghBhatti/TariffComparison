using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using TariffComparison.Models;

namespace TariffComparison.Providers
{
    public class TariffProvider : ITariffProvider
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5); // Set cache duration
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TariffProvider> _logger;

        public TariffProvider(IMemoryCache memoryCache, ILogger<TariffProvider> logger)
        {
            this._memoryCache = memoryCache;
            this._logger = logger;
        }

        public async Task<IEnumerable<TariffModel>> GetTariffsAsync()
        {
            var tariffsJson = File.ReadAllText("./tariffs.json");
            if (string.IsNullOrWhiteSpace(tariffsJson))
                return [];

            // Check if the tariffs are already in the cache
            if (_memoryCache.TryGetValue("Tariffs", out IEnumerable<TariffModel> cachedTariffs))
            {
                return cachedTariffs; // Return cached value
            }

            var rawTariffs = JArray.Parse(tariffsJson);

            if (rawTariffs == null || !rawTariffs.Any())
                return [];

            var tariffs = new List<TariffModel>();
            try
            {
                foreach (var rawTariff in rawTariffs)
                {
                    var baseTariff = new TariffModel
                    {
                        Name = rawTariff["name"]?.ToString(),
                        Type = Convert.ToInt32(rawTariff["type"]),
                        BaseCost = rawTariff["baseCost"]?.Value<decimal>() ?? 0,
                        AdditionalProperties = new Dictionary<string, object>()
                    };

                    foreach (var property in rawTariff.Children<JProperty>())
                    {

                        if (property.Name != "name" && property.Name != "type" && property.Name != "baseCost")
                        {
                            baseTariff.AdditionalProperties[property.Name] = property.Value.Type switch
                            {
                                JTokenType.Integer => property.Value.Value<int>(),
                                JTokenType.Float => property.Value.Value<decimal>(),
                                JTokenType.String => property.Value.Value<string>(),
                                JTokenType.Null => null,
                                _ => property.Value.ToString()
                            };
                        }
                    }
                    tariffs.Add(baseTariff);
                }
            } catch(Exception ex)
            {
                _logger.LogError($"Error occured while parsing tariffs json::{ex}");
                throw;
            }

            // Cache the data with an expiration time
            _memoryCache.Set("Tariffs", tariffs, CacheDuration);
            return await Task.FromResult(tariffs);
        }
    }
}