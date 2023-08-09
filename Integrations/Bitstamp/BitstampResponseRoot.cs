using System.Text.Json.Serialization;

namespace Integrations.Bitstamp
{
    internal class BitstampResponseRoot
    {
        [JsonPropertyName("data")]
        public BitstampResponseData Data { get; set; }
    }
}