using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class PriceRange
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

    public class ProductFilterContract
    {
        public HashSet<string> GenderFilters { get; set; } = new HashSet<string>();
        public HashSet<string> BrandFilters { get; set; } = new HashSet<string>();
        public HashSet<int> SizeFilters { get; set; } = new HashSet<int>();
        public PriceRange PriceRange { get; set; } = new PriceRange { Min = 0, Max = decimal.MaxValue };
        public string SearchTerm { get; set; } = string.Empty;
        public string SortOption { get; set; } = "az";
        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 12;
    }
}
