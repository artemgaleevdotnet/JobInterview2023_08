using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<BitcoinPrice> BitcoinPrices { get; set; }
    }
}