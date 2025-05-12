using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class CreateProductContract
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ColorId { get; set; }
    }
}
