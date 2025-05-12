using Microsoft.EntityFrameworkCore;
using ShoeStore.Repository.Interfaces;
using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Implementions
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoeStoreDbContext _context;

        public ProductRepository(ShoeStoreDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p => p.DeletedAt == null)
                .ToList();
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            
            await _context.SaveChangesAsync();
        }
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Include(p => p.ProductVersions)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.DeletedAt == null);
        }
    }
}
