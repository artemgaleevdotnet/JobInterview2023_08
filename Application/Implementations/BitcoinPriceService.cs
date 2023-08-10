using Application.Abstractions;
using DomainModels;
using Integrations.Common;
using Persistance;

namespace Application.Implementations
{
    internal class BitcoinPriceService : IBitcoinPriceService
    {
        private readonly IBitcoinPriceDataStore _dataStore;
        private readonly IDataAggregator _dataAggregator;
        private readonly IEnumerable<IBitcoinPriceFetcher> _dataFetchers;

        public BitcoinPriceService(IBitcoinPriceDataStore dataStore, IDataAggregator dataAggregator,
            IEnumerable<IBitcoinPriceFetcher> dataFetchers)
        {
            _dataStore = dataStore;
            _dataFetchers = dataFetchers;
            _dataAggregator = dataAggregator;
        }

        public Task<IReadOnlyCollection<IBitcoinPrice>> GetPricesAsync(DateTimeOffset startDate, DateTimeOffset endDate) =>
            _dataStore.GetPricesAsync(startDate, endDate);

        public async Task<IBitcoinPrice?> GetPriceAsync(DateTimeOffset timePoint)
        {
            var price = await _dataStore.GetPriceAsync(timePoint);

            if (price != null)
            {
                return price;
            }

            var tasks = _dataFetchers.Select(dataFetcher => dataFetcher.GetCandle(timePoint)).ToArray();

            // Implemented all or nothing scenario, second option is to continue work with not failed tasks,
            // but second option cause a lot of questions like:
            //      should we store price to DB if not all sources (fetchers) available?
            //      should we add ability to update stored values?
            //      Then we should update and then we shouldn't (then all sources available or every time then values are different)?
            //      and so on. It's the reason to keep it simple.
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (IntegrationException ex)
            {
                // logs

                return null;
            }
            catch (Exception ex)
            {
                // logs

                return null;
            }

            var candles = tasks.Select(t => t.Result);

            if (!candles.Any())
            {
                return null;
            }

            var aggregatedPrice = _dataAggregator.Aggregate(candles.ToArray());

            var bitcoinPrice = new BitcoinPrice
            {
                Price = aggregatedPrice,
                TimePoint = timePoint
            };

            return await _dataStore.SavePriceAsync(bitcoinPrice);
        }
    }
}