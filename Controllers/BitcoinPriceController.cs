using BitcoinPrices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BitcoinPrices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinPriceController : ControllerBase
    {
        private readonly PricesContext _context;

        public BitcoinPriceController(PricesContext context)
        {
            _context = context;
        }

        // GET: api/<BitcoinPriceController>
        [HttpGet("GetCurrentPrice")]
        public async Task<ActionResult<BitcoinPriceDTO>> GetCurrentPrice()
        {
            var price = await _context.BitcoinPrices.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            var allPrices = _context.BitcoinPrices.Count();

            if (price == null)
            {
                return NotFound();
            }

            return new BitcoinPriceDTO() { Price = price.Price, FromDate = price.CreatedAt };
        }

        // GET api/<BitcoinPriceController>/5
        [HttpGet("GetAveragePriceForDate/{date}")]
        public async Task<ActionResult<BitcoinPriceDTO>> GetAveragePriceForDate(DateTime date)
        {
            var start = date;
            var end = date.AddDays(1).AddSeconds(-1);
            var prices = await _context.BitcoinPrices.OrderByDescending(p => p.Id).Where(d => d.CreatedAt >= start && d.CreatedAt <= end).ToListAsync();
            int count = prices.Count;
            double sum = 0;
            foreach (var pr in prices) sum += pr.Price;
            double average = Math.Round(sum / count, 2);
            return new BitcoinPriceDTO() { Price = average, FromDate = date };
        }

        // GET api/<BitcoinPriceController>/5
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<BitcoinPrice>>> GetAll(int id)
        {
            var prices = await _context.BitcoinPrices.OrderByDescending(p => p.Id).ToListAsync();

            if (prices == null)
            {
                return NotFound();
            }

            return prices;
        }

        // DELETE api/<BitcoinPriceController>/5
        [HttpDelete("RemoveAll")]
        public void Delete()
        {
            _context.BitcoinPrices.RemoveRange(_context.BitcoinPrices);
            _context.SaveChanges();
        }
    }
}
