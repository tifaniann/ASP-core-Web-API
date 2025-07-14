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

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<StockDto>> GetAllAsyncDto()
        {
            return await _context.Stocks
                .Select(s => s.ToStockDto())
                .ToListAsync();
            //s(alias semacam i dalam looping): Untuk setiap data Stock yang diambil, ubah menjadi StockDto menggunakan metode ekstensi ToStockDto() yang ada di Mappers/StockMappers.cs
            // _context: Ambil semua data dari tabel Stocks di database, dan ubah jadi List<Stock>
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(c => c.Comments).ToListAsync();
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
            stockDto.MapToExistingStock(stock);
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

    }
}