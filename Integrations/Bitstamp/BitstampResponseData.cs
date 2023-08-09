using System.Text.Json.Serialization;

namespace Integrations.Bitstamp
{
    internal class BitstampResponseData
    {
        [JsonPropertyName("ohlc")]
        public List<BitstampResponseOhlc> Ohlc { get; set; }
        [JsonPropertyName("pair")]
        public string Pair { get; set; }
    }
}