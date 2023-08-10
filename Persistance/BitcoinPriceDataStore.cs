using DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    internal class BitcoinPriceDataStore : IBitcoinPriceDataStore
    {
        private readonly AppDbContext _appDbContext;

        public BitcoinPriceDataStore(AppDbContext appDbContext)
        { 
            _appDbContext = appDbContext;
        }

        public async Task<IBitcoinPrice?> GetPriceAsync(DateTimeOffset timePoint) =>
            await _appDbContext.BitcoinPrices.FirstOrDefaultAsync(bp => bp.TimePoint == timePoint);

        public async Task<IReadOnlyCollection<IBitcoinPrice>> GetPricesAsync(DateTimeOffset startDate, DateTimeOffset endDate) =>
            await _appDbContext.BitcoinPrices
                .Where(bp => bp.TimePoint >= startDate && bp.TimePoint <= endDate)
                .OrderBy(bp => bp.TimePoint)
                .ToArrayAsync();

        public async Task<IBitcoinPrice> SavePriceAsync(IBitcoinPrice bitcoinPrice)
        {
            var existsEntry = await GetPriceAsync(bitcoinPrice.TimePoint);

            if (existsEntry != null)
            {
                return existsEntry;
            }

            var entity = new BitcoinPrice(bitcoinPrice);

            _appDbContext.BitcoinPrices.Add(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}