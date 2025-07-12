using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOS.Stock;
using api.Models;

namespace api.Interfaces
{
    // Antarmuka ini mendefinisikan kontrak untuk repositori yang mengelola entitas Stock.
    // Repositori ini akan digunakan untuk mengakses data Stock dari database.
    //interface digunakan untuk kelas yang memiliki perilaku yang sama
    public interface IStockRepository
    {
        Task<List<StockDto>> GetAllAsyncDto();
        Task<List<Stock>> GetAllAsync();
        Task<StockDto?> GetByIdAsync(int id);
        Task<StockDto> CreateAsyncDto(CreateStockRequestDto Stockdto);
        Task<StockDto?> UpdateAsyncDto(int id, UpdateStockRequestDto Stockdto);
        Task<bool> DeleteAsync(int id);
    }
}