using System.Text.Json.Serialization;

namespace Integrations.Bitstamp
{
    internal class BitstampResponseOhlc
    {
        [JsonPropertyName("close")]
        public string Close { get; set; }
        [JsonPropertyName("high")]
        public string High { get; set; }
        [JsonPropertyName("low")]
        public string Low { get; set; }
        [JsonPropertyName("open")]
        public string Open { get; set; }
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("volume")]
        public string Volume { get; set; }
    }
}