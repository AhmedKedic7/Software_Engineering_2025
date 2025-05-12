using ShoeStore.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductContract> AddProductAsync(CreateProductContract createProductContract);
        Task<ProductContract?> GetProductByIdAsync(Guid id);
        IEnumerable<ProductContract> GetAllProducts();
    }
}
