using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOS.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    //controller ini akan menangani permintaan HTTP untuk entitas Stock. 
    // controller adalah class yang menangani permintaan HTTP dan mengembalikan respons HTTP.
    // kelas yang digunakan untuk mengelola data Stock ke database melalui api
    [Route("api/stock")]
    [ApiController]
    //dua diatas wajib ada di setiap controller, fungsinya untuk menyambungkan controller dengan route
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockDtoRepo;
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context, IStockRepository stockDtoRepo) // constructor untuk menginisialisasi context dan repository
        {
            _stockDtoRepo = stockDtoRepo;
            _context = context;

        }

        [HttpGet] // untuk mengambil
        public async Task<IActionResult> GetAllStock() //IActionResult Adalah interface. interface digunakan untuk kelas yang memiliki perilaku yang sama
        {
            var stocks = await _stockDtoRepo.GetAllAsyncDto();
            return Ok(stocks); // Kembalikan data dengan status 200 OK. 200 OK adalah status HTTP yang menunjukkan bahwa permintaan berhasil diproses.
        }

        [HttpGet("stock/{id}")] // untuk mengambil berdasarkan id, contoh GET http://localhost:5000/stock/7

        //async digunakan untuk mempercepat proses pengambilan data dari database
        //async await digunakan untuk mempercepat proses pengambilan data dari database/kode
        public async Task<IActionResult> GetStockById([FromRoute] int id)  // [FromRoute] digunakan untuk mengambil nilai dari route, int id adalah parameter yang akan diambil dari URL
        {
            //await digunakan untuk menunggu proses pengambilan data dari database selesai
            // sehingga tidak akan menghalangi thread utama aplikasi; agar controller nggak macet
            var stock = await _context.Stocks.FindAsync(id); // _context: Ambil data dari tabel Stocks berdasarkan id yang diberikan
            if (stock == null)
            {
                return NotFound(); // Jika tidak ditemukan, kembalikan status 404 Not Found
            }
            return Ok(stock.ToStockDto()); // Jika ditemukan, kembalikan data dengan status 200 OK
        }

        [HttpPost] // untuk menambahkan data baru
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto) // [FromBody] digunakan untuk mengambil data dari body request. jadi kita mengambil data dari body request kemudian dimasukkan ke dalam database
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stockModel); // _context: Tambahkan data Stock baru ke tabel Stocks di database
            await _context.SaveChangesAsync(); // Simpan perubahan ke database
            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto()); // Kembalikan status 201 Created dan lokasi dari data yang baru dibuat

        }

        [HttpPut("{id}")] // untuk mengupdate data berdasarkan id, contoh PUT http://localhost:5000/stock/7
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto) // [FromRoute] digunakan untuk mengambil nilai dari route, int id adalah parameter yang akan diambil dari URL
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id); // _context: Ambil data dari tabel Stocks berdasarkan id yang diberikan
            if (stockModel == null)
            {
                return NotFound(); // Jika tidak ditemukan, kembalikan status 404 Not Found
            }

            UpdateDto.MapToExistingStock(stockModel);
            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDto()); // Jika ditemukan, ubah data Stock yang ada dengan data yang baru dan kembalikan status 200 OK
        }

        [HttpDelete("{id}")] // untuk menghapus data berdasarkan id, contoh DELETE http://localhost:5000/stock/7
        public async Task<IActionResult> DeleteStock([FromRoute] int id) // [FromRoute] digunakan untuk mengambil nilai dari route, int id adalah parameter yang akan diambil dari URL
        {
            var stockModel = await _context.Stocks.FindAsync(id); // _context: Ambil data dari tabel Stocks berdasarkan id yang diberikan
            if (stockModel == null)
            {
                return NotFound(); // Jika tidak ditemukan, kembalikan status 404 Not Found
            }
            //async tidak ada di remove karena remove tidak membutuhkan proses yang lama
            _context.Stocks.Remove(stockModel); // _context: Hapus data Stock dari tabel Stocks di database
            await _context.SaveChangesAsync();
            return NoContent(); // Kembalikan status 204 No Content, yang berarti permintaan berhasil diproses tetapi tidak ada konten yang dikembalikan
        }
    }
}