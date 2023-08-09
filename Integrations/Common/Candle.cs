using DomainModels;

namespace Integrations.Common
{
    internal class Candle : ICandle
    {
        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal Low { get; set; }

        public decimal High { get; set; }

        public decimal Volume { get; set; }

        public long Timestamp { get; set; }
    }
}