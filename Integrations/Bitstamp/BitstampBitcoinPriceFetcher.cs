using DomainModels;
using Integrations.Common;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Integrations.Bitstamp
{
    public class BitstampBitcoinPriceFetcher : IBitcoinPriceFetcher
    {
        private readonly ILogger<BitstampBitcoinPriceFetcher> _logger;
        private readonly HttpClient _httpClient;

        public BitstampBitcoinPriceFetcher(
            ILogger<BitstampBitcoinPriceFetcher> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient(nameof(BitstampBitcoinPriceFetcher));
        }

        public async Task<ICandle> GetCandle(DateTimeOffset timePoint)
        {
            using HttpRequestMessage message =
                new(HttpMethod.Get, $"?step=3600&limit=1&start={timePoint.ToUnixTimeSeconds()}");

            using var response = await _httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Time point={timePoint.ToUnixTimeSeconds()}. Request failed with status code: {response.StatusCode}. {response.ReasonPhrase}.");

                throw new IntegrationException(
                    $"Request failed with status code: {response.StatusCode}. {response.ReasonPhrase}.");
            }
            string content = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<BitstampResponseRoot>(content);

            if (responseData == null || responseData.Data == null)
            {
                _logger.LogWarning($"Time point={timePoint.ToUnixTimeSeconds()}. Deserialization failed or response is empty.");

                throw new IntegrationException($"Deserialization failed or response is empty.");
            }

            if (responseData.Data.Ohlc == null || !responseData.Data.Ohlc.Any())
            {
                _logger.LogWarning($"Time point={timePoint.ToUnixTimeSeconds()}. No data for {responseData.Data.Pair}.");

                throw new IntegrationException($"No data for {responseData.Data.Pair}.");
            }

            var ohlc = responseData.Data.Ohlc.SingleOrDefault();

            if (responseData.Data.Ohlc.Count > 1)
            {
                _logger.LogWarning($"Time point={timePoint.ToUnixTimeSeconds()}. More date than should be.");

                throw new IntegrationException($"More date than should be.");
            }

            return new Candle
            {
                Volume = TryParseValue(responseData.Data.Ohlc[0].Volume, nameof(Candle.Volume)),
                Timestamp = (long)TryParseValue(responseData.Data.Ohlc[0].Timestamp, nameof(Candle.Timestamp)),
                High = TryParseValue(responseData.Data.Ohlc[0].High, nameof(Candle.High)),
                Open = TryParseValue(responseData.Data.Ohlc[0].Open, nameof(Candle.Open)),
                Close = TryParseValue(responseData.Data.Ohlc[0].Close, nameof(Candle.Close)),
                Low = TryParseValue(responseData.Data.Ohlc[0].Low, nameof(Candle.Low))
            };
        }

        private decimal TryParseValue(string value, string fieldName)
        {
            if (!decimal.TryParse(value, out var parsedValue))
            {
                _logger.LogError($"Can't parse {fieldName} value: {value}.");

                throw new IntegrationException($"Can't parse {fieldName} value: {value}.");
            }
            return parsedValue;
        }
    }
}