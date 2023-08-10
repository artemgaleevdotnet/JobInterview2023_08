using Application.Abstractions;
using DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BitcoinPricesController : Controller
    {
        private readonly IBitcoinPriceService _bitcoinPriceService;

        public BitcoinPricesController(IBitcoinPriceService bitcoinPriceService)
        {
            _bitcoinPriceService = bitcoinPriceService;
        }

        [HttpGet("{timePoint}")]
        [CleanUpMinutesSeconds]
        public async Task<ActionResult<IBitcoinPrice>> Get(DateTimeOffset timePoint)
        {
            if (timePoint > DateTimeOffset.UtcNow)
            {
                return BadRequest($"Wrong request. It's future date: {timePoint}.");
            }

            var price = await _bitcoinPriceService.GetPriceAsync(timePoint);

            if (price == null)
            {
                return StatusCode(503, $"Source of data unavailable or returns empty result.");
            }

            return Ok(price);
        }

        [HttpGet]
        [CleanUpMinutesSeconds]
        public async Task<ActionResult<IReadOnlyCollection<IBitcoinPrice>>> GetList(
            [FromQuery] DateTimeOffset startTimePoint, [FromQuery] DateTimeOffset endTimePoint)
        {
            if (startTimePoint > endTimePoint)
            {
                return BadRequest($"Start date can't be bigger or equals to end date.");
            }

            var prices = await _bitcoinPriceService.GetPricesAsync(startTimePoint, endTimePoint);

            return Ok(prices);
        }
    }
}
