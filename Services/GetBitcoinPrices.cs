using BitcoinPrices.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BitcoinPrices.Services
{
    public class GetBitcoinPrices : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetBitcoinPrices(IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                int minuteSpan = 60 - DateTime.Now.Minute; // Minutes till next hour
                await Task.Delay(TimeSpan.FromMinutes(minuteSpan), stoppingToken);
                //await Task.Delay(TimeSpan.FromSeconds(minuteSpan), stoppingToken);
                await GetPrice();
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task GetPrice()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            PricesContext _priceContext = scope.ServiceProvider.GetRequiredService<PricesContext>();
            Console.WriteLine(_priceContext);
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync("https://api.coinlore.net/api/ticker/?id=90");
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                var dataResponse = JsonConvert.DeserializeObject<BitcoinPriceResponseModel[]>(response);
                var dataPrice = dataResponse[0];
                Console.WriteLine(dataPrice.price_usd);
                if (dataResponse.Length == 0) return;
                BitcoinPrice newPrice = new BitcoinPrice
                {
                    Price = dataPrice.price_usd,
                    CreatedAt = DateTime.Now,
                };

                Console.WriteLine("New price: " + newPrice.Price);

                _priceContext.BitcoinPrices.Add(newPrice);

                try
                {
                    await _priceContext.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (PriceExists(_priceContext, newPrice.Id))
                    {
                        Console.WriteLine("Price already exists!");
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        }

        private bool PriceExists(PricesContext context, long id)
        {
            return context.BitcoinPrices.Any(e => e.Id == id);
        }
    }
}
