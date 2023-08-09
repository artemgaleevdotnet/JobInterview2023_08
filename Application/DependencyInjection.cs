using Application.Abstractions;
using Application.Implementations;
using Integrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Persistance;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDataAggregator, DataAggregatorVersionOne>();
            services.AddScoped<IBitcoinPriceService, BitcoinPriceService>();
            services.AddPersistance();
            services.AddIntegrations(configuration);
        }
    }
}