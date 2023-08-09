using DomainModels;
using Integrations.Common;
using System.Text.Json;

namespace Integrations.Bitfinex
{
    public class BitfinexBitcoinPriceFetcher : IBitcoinPriceFetcher
    {
        private readonly HttpClient _httpClient;

        public BitfinexBitcoinPriceFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(BitfinexBitcoinPriceFetcher));
        }

        public async Task<ICandle> GetCandle(DateTimeOffset timePoint)
        {
            using HttpRequestMessage message =
                new(HttpMethod.Get, $"?start={timePoint.ToUnixTimeMilliseconds()}&limit=1");

            using var response = await _httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                // logs
                throw new IntegrationException(
                    $"Request failed with status code: {response.StatusCode}. {response.ReasonPhrase}.");
            }
            string content = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<decimal[][]>(content)
                ?? throw new IntegrationException($"Deserialization failed or response is empty.");

            if (responseData.Length != 1 || responseData[0].Length != 6)
            {
                // logs
                throw new IntegrationException($"Wrong data.");
            }

            return new Candle
            {
                Timestamp = (long)responseData[0][0],
                Open = responseData[0][1],
                Close = responseData[0][2],
                High = responseData[0][3],
                Low = responseData[0][4],
                Volume = responseData[0][5]
            };
        }
    }
}