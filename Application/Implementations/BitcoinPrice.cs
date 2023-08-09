using DomainModels;

namespace Application.Implementations
{
    internal class BitcoinPrice : IBitcoinPrice
    {
        public decimal Price { get; set; }
        public DateTimeOffset TimePoint { get; set; }
    }
}