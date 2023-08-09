using DomainModels;

namespace Application.Abstractions
{
    public interface IDataAggregator
    {
        public decimal Aggregate(IReadOnlyCollection<ICandle> candles);
    }
}