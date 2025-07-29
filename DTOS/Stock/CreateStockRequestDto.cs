using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOS.Stock
{
    public class CreateStockRequestDto
    {
        // get bisa dibaca; set bisa ditulis
        [Required]
        [StringLength(10, ErrorMessage = "Symbol must be 10 characters or less.")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [StringLength(10, ErrorMessage = "Company name must be 10 characters or less.")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000, ErrorMessage = "Purchase must be between 1 and 10000.")]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100, ErrorMessage = "Last Div must be between 0.001 and 100.")]
        public decimal LastDiv { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Industry must be 50 characters or less.")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000, ErrorMessage = "Market Cap must be between 1 and 1000000000.")]
        public long MarketCap { get; set; }
    }
}