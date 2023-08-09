using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistance
{
    public static class DependencyInjection
    {
        public static void AddPersistance(this IServiceCollection services)
        {
            services.AddScoped<IBitcoinPriceDataStore, BitcoinPriceDataStore>();

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("BitcoinPrices"));
        }
    }
}