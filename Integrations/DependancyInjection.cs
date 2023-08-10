using Integrations.Bitfinex;
using Integrations.Bitstamp;
using Integrations.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Integrations
{
    public static partial class DependancyInjection 
    {
        public static void AddIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            /*
                from requirements: "In the future the system may evolve to fetch data from many more sources"
                we need to add more sources of data that will base on IBitcoinPriceFetcher
                and register, it will automatically applied
             */

            services.AddScoped<IBitcoinPriceFetcher, BitstampBitcoinPriceFetcher>();
            services.AddScoped<IBitcoinPriceFetcher, BitfinexBitcoinPriceFetcher>();

            services.AddHttpClient(nameof(BitstampBitcoinPriceFetcher), client =>            
            {
                var baseUrl = configuration["BitstampBaseUrl"];
                if(string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new ConfigurationException("BitstampBaseUrl not found or empty.");
                }
                client.BaseAddress = new Uri(baseUrl);
            });
            services.AddHttpClient<BitfinexBitcoinPriceFetcher>(client =>
            {
                var baseUrl = configuration["BitfinexBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new ConfigurationException("BitfinexBaseUrl not found or empty.");
                }
                client.BaseAddress = new Uri(baseUrl);
            });
        }
    }
}