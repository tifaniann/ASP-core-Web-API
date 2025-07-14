using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOS.Comment;

namespace api.DTOS.Stock
{
    // DTO (Data Transfer Object) adalah objek yang digunakan untuk mentransfer data antara lapisan aplikasi
    // DTO berfungsi Memisahkan data yang dikirim atau diterima dari bentuk asli di database
    //Menghindari pengiriman data yang tidak perlu (misal: password, token).
    public class StockDto
    {
        // get bisa dibaca; set bisa ditulis
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}