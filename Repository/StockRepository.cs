using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Interfaces;
using api.Models;
using api.DTOS.Stock;
using api.Mappers;
using Microsoft.EntityFrameworkCore;
using api.Helper;
using static api.Helper.QueryObject;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<StockDto>> GetAllAsyncDto(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.Industry))
            {
                stocks = stocks.Where(s => s.Industry.Contains(query.Industry));
            }

            // dipake kalo input query.SortBy (JANGAN DIHAPUS)
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)) //StringComparison.OrdinalIgnoreCase = Bandingkan dua string tapi ignore lower/upper case
                {
                    stocks = query.isDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol); //if else if true : false
                }
                else if (query.SortBy.Equals("Industry", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.isDescending ? stocks.OrderByDescending(s => s.Industry) : stocks.OrderBy(s => s.Industry); //if else if true : false
                }
            }

            // switch (query.SortingBy)
            // {
            //     case SortOption.Symbol:
            //         stocks = query.isDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            //         break;
            //     case SortOption.Industry:
            //         stocks = query.isDescending ? stocks.OrderByDescending(s => s.Industry) : stocks.OrderBy(s => s.Industry);
            //         break;
            // }

            return await stocks.Select(s => s.ToStockDto()).ToListAsync();
            //s(alias semacam i dalam looping): Untuk setiap data Stock yang diambil, ubah menjadi StockDto menggunakan metode ekstensi ToStockDto() yang ada di Mappers/StockMappers.cs
            // _context: Ambil semua data dari tabel Stocks di database, dan ubah jadi List<Stock>
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
            // Mengambil semua data dari tabel Stocks di database
        }

        public async Task<StockDto?> GetByIdAsync(int id)
        {
            // _context: Ambil data dari tabel Stocks berdasarkan id yang diberikan
            var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id); 
            return stock?.ToStockDto();
            // Mengambil data Stock berdasarkan id, jika ditemukan, ubah menjadi StockDto
        }

        public async Task<StockDto> CreateAsyncDto(CreateStockRequestDto stockDto)
        {
            int maxId = await _context.Stocks.MaxAsync(c => (int?)c.Id) ?? 0;

            // Reset IDENTITY untuk ambil id maksimum yang ada di tabel Stocks
            await _context.Database.ExecuteSqlInterpolatedAsync($"DBCC CHECKIDENT ('Stocks', RESEED, {maxId})");

            var stockModel = stockDto.ToStockFromCreateDto(); 
            await _context.Stocks.AddAsync(stockModel); // _context: Tambahkan data Stock baru ke tabel Stocks di database
            await _context.SaveChangesAsync(); // Simpan perubahan ke database
            return stockModel.ToStockDto();
        }

        public async Task<StockDto?> UpdateAsyncDto(int id, UpdateStockRequestDto stockDto)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return null; // Jika tidak ditemukan, kembalikan null
            }
            stockDto.MapToExistingStock(stock); // 
            await _context.SaveChangesAsync();
            return stock.ToStockDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return false; 
            }
            _context.Stocks.Remove(stock); // Hapus data Stock dari tabel Stocks di database
            await _context.SaveChangesAsync(); // Simpan perubahan ke database
            return true;
        }

        public Task<bool> IsStockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
            // Mengecek apakah ada data Stock dengan id yang diberikan
        }
    }
}