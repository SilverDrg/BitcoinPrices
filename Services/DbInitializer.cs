using BitcoinPrices.Helper;
using BitcoinPrices.Models;

namespace BitcoinPrices.Services
{
    internal class DbInitializer
    {
        internal static void Initialize(PricesContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();
            if (dbContext.BitcoinPrices.Any()) return;

            var prices = new List<BitcoinPrice>();
            int days = 14;
            int hours = 24;
            int totalObjects = days * hours;

            int dayIteration = 0;
            int hourIteration = 0;
            Random random = new Random();

            for (int i = 0; i < totalObjects; i++)
            {
                if (hourIteration == hours)
                {
                    dayIteration++;
                    hourIteration = 0;
                }
                DateTime current = DateTime.Now;
                DateTime updated = current.Subtract(new TimeSpan(dayIteration, hourIteration, 0, 0));
                var newPrice = new BitcoinPrice { CreatedAt = updated, Price = Math.Round(RandomExtensions.NextDouble(random, 16651.71, 36722.46), 2) };
                prices.Add(newPrice);

                hourIteration++;
            }

            Console.WriteLine(prices.ToString());

            dbContext.BitcoinPrices.AddRange(prices);
            dbContext.SaveChanges();

        }
    }
}
