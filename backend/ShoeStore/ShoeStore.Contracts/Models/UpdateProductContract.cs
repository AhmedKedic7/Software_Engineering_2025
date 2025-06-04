using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class UpdateProductContract
    {
        public Guid ProductId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public string Gender { get; set; }
        public List<ImageRequest> Images { get; set; }
        public class ImageRequest
        {
            public string ImageUrl { get; set; }
            public bool IsMain { get; set; }
        }
    }
}
