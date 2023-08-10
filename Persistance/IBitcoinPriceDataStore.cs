using DomainModels;

namespace Persistance
{
    public interface IBitcoinPriceDataStore
    {
        public Task<IBitcoinPrice?> GetPriceAsync(DateTimeOffset timePoint);
        public Task<IReadOnlyCollection<IBitcoinPrice>> GetPricesAsync(DateTimeOffset startDate, DateTimeOffset endDate);
        public Task<IBitcoinPrice> SavePriceAsync(IBitcoinPrice bitcoinPrice);
    }
}