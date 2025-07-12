using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOS.Stock;
using api.Models;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }

        public static void MapToExistingStock(this UpdateStockRequestDto dto, Stock existingStock)
        {
            existingStock.Symbol = dto.Symbol;
            existingStock.CompanyName = dto.CompanyName;
            existingStock.Purchase = dto.Purchase;
            existingStock.LastDiv = dto.LastDiv;
            existingStock.Industry = dto.Industry;
            existingStock.MarketCap = dto.MarketCap;
        }

    }
}