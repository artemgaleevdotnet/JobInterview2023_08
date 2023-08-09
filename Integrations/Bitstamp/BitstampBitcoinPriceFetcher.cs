using DomainModels;
using Integrations.Common;
using System.Text.Json;

namespace Integrations.Bitstamp
{
    public class BitstampBitcoinPriceFetcher : IBitcoinPriceFetcher
    {
        private readonly HttpClient _httpClient;

        public BitstampBitcoinPriceFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(BitstampBitcoinPriceFetcher));
        }

        public async Task<ICandle> GetCandle(DateTimeOffset timePoint)
        {
            using HttpRequestMessage message =
                new(HttpMethod.Get, $"?step=3600&limit=1&start={timePoint.ToUnixTimeSeconds()}");

            using var response = await _httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                // logs
                throw new IntegrationException(
                    $"Request failed with status code: {response.StatusCode}. {response.ReasonPhrase}.");
            }
            string content = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<BitstampResponseRoot>(content);

            if (responseData == null || responseData.Data == null)
            {
                // logs
                throw new IntegrationException($"Deserialization failed or response is empty.");
            }

            if (responseData.Data.Ohlc == null || !responseData.Data.Ohlc.Any())
            {
                // logs
                throw new IntegrationException($"No data for {responseData.Data.Pair}.");
            }

            var ohlc = responseData.Data.Ohlc.SingleOrDefault();

            if (responseData.Data.Ohlc.Count > 1)
            {
                // logs
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

        private static decimal TryParseValue(string value, string fieldName)
        {
            if (!decimal.TryParse(value, out var parsedValue))
            {
                throw new IntegrationException($"Can't parse {fieldName} value: {value}.");
            }
            return parsedValue;
        }
    }
}