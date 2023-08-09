using DomainModels;
using System.ComponentModel.DataAnnotations;

namespace Persistance
{
    internal class BitcoinPrice : IBitcoinPrice
    {
        public BitcoinPrice() { }
        public BitcoinPrice(IBitcoinPrice bitcoinPrice)
        {
            Price = bitcoinPrice.Price;
            TimePoint = bitcoinPrice.TimePoint;
        }

        [Key]
        public DateTimeOffset TimePoint { get; set; }

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
        public decimal Price { get; set; }
    }
}