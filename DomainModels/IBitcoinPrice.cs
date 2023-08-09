namespace DomainModels
{
    public interface IBitcoinPrice
    {
        public decimal Price { get; }
        public DateTimeOffset TimePoint { get; }
    }
}