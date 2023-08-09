using DomainModels;

namespace Application.Abstractions
{
    public interface IBitcoinPriceService
    {
        public Task<IBitcoinPrice?> GetPriceAsync(DateTimeOffset timePoint);
        public Task<IReadOnlyCollection<IBitcoinPrice>> GetPricesAsync(DateTimeOffset startDate, DateTimeOffset endDate);
    }
}