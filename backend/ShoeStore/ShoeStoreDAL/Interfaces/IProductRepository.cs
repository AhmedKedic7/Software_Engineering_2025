using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStore.Contracts.Models;

namespace ShoeStore.Repository.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Task AddProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<int> GetLatestProductVersionAsync(Guid productId);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredProductsAsync(ProductFilterContract filterDto);

        Task<SoldProductContract> GetSoldProductsAsync(Guid productId);

        Task AddAsync(Image image);

        Task<Product> UpdateDeletedAtAsync(Guid productId, int version, DateTime? deletedAt);
        Task<List<Product>> GetLatestProductsAsync(int count);

        Task<bool> LockProductAsync(Guid productId, Guid adminId);
        Task<bool> UnlockProductAsync(Guid productId, Guid adminId);

    }
}
