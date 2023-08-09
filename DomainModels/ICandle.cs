namespace DomainModels
{
    public interface ICandle
    {
        public decimal Open { get; }
        public decimal Close { get; }
        public decimal Low { get; }
        public decimal High { get; }
        public decimal Volume { get; }
        public long Timestamp { get; }
    }
}