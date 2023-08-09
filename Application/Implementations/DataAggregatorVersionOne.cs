using Application.Abstractions;
using DomainModels;

namespace Application.Implementations
{
    /*
      from requirements:
     " or change the price aggregation formula to another more complex one".

        For change formula we need create new class and change registration in DependencyInjection.cs
     */

    public class DataAggregatorVersionOne : IDataAggregator
    {
        public decimal Aggregate(IReadOnlyCollection<ICandle> candles) => candles.Average(candle => candle.Close);
    }
}