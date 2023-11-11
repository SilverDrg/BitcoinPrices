using Microsoft.EntityFrameworkCore;

namespace BitcoinPrices.Models
{
    public class PricesContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public PricesContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));

        public DbSet<BitcoinPrice> BitcoinPrices { get; set; }
    }
}
