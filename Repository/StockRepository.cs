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

        public async Task<List<StockDto>> GetAllAsync()
        {
            return await _context.Stocks
                .Select(s => s.ToStockDto())
                .ToListAsync();
            //s(alias semacam i dalam looping): Untuk setiap data Stock yang diambil, ubah menjadi StockDto menggunakan metode ekstensi ToStockDto() yang ada di Mappers/StockMappers.cs
            // _context: Ambil semua data dari tabel Stocks di database, dan ubah jadi List<Stock>
        }

    }
}