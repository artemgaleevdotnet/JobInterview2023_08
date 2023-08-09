using DomainModels;

namespace Integrations.Common
{
    public interface IBitcoinPriceFetcher
    {
        public Task<ICandle> GetCandle(DateTimeOffset timePoint);
    }
}