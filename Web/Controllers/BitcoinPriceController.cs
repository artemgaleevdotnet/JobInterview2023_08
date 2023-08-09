using Application.Abstractions;
using DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitcoinPriceController : Controller
    {
        private readonly IBitcoinPriceService _bitcoinPriceService;

        public BitcoinPriceController(IBitcoinPriceService bitcoinPriceService)
        {
            _bitcoinPriceService = bitcoinPriceService;
        }

        [HttpGet]
        [Route("GetPrice")]
        [CleanUpMinutesSeconds]
        public async Task<ActionResult<IBitcoinPrice>> Get([FromQuery] DateTimeOffset timePoint)
        {
            if (timePoint > DateTimeOffset.UtcNow)
            {
                return BadRequest($"Wrong request. It's future date: {timePoint}.");
            }

            var price = await _bitcoinPriceService.GetPriceAsync(timePoint);

            if (price == null)
            {
                return StatusCode(500, $"Internal Server Error");
            }

            return Ok(price);
        }

        [HttpGet("GetPrices")]
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
