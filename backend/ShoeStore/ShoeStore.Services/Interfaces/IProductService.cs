using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
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

        Task<(IEnumerable<ProductContract> Products, int TotalCount)> GetFilteredProductsAsync(ProductFilterContract filterDto);

        Task<SoldProductContract> GetSoldProductsAsync(Guid productId);

        Task AddImageAsync(Image image);
        Task<Product> CreateNewVersionAsync(UpdateProductContract request);

        Task<Product> UpdateDeletedAtAsync(Guid productId, int version, DateTime? deletedAt);
        Task<List<ProductContract>> GetLatestProductsAsync(int count);

        Task<bool> LockProductAsync(Guid productId, Guid adminId);
        Task<bool> UnlockProductAsync(Guid productId, Guid adminId);
    }
}
