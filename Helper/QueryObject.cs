using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helper
{
    public class QueryObject
    {
        public string? CompanyName { get; set; } = null;
        public string? Symbol { get; set; } = null;
        public string? Industry { get; set; } = null;
        public enum SortOption
        {
            Symbol,
            Industry
        }
        // public string? SortBy { get; set; } = null;
        public SortOption SortingBy { get; set; } = SortOption.Symbol;
        public bool isDescending { get; set; } = false;
    }
}