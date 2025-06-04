using Microsoft.EntityFrameworkCore;
using ShoeStore.Contracts.Models;
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

        public async Task<int> GetLatestProductVersionAsync(Guid productId)
        {
            // Get the latest version of the product by ProductId
            var latestVersion = await _context.Products
                .Where(p => p.ProductId == productId && p.DeletedAt == null)
                .MaxAsync(p => p.Version);

            return latestVersion;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p=>p.IsLast==true)
                .ToList();
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            
            await _context.SaveChangesAsync();
        }


        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var latestVersion = await _context.Products
            .Where(p => p.ProductId == id && p.DeletedAt == null)
            .MaxAsync(p => p.Version);


            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id && p.DeletedAt == null && p.Version == latestVersion);
        }


        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredProductsAsync(ProductFilterContract filterDto)
        {
            var query = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p => p.DeletedAt == null && p.IsLast==true)
                .AsQueryable();

            // Apply filters
            if (filterDto.GenderFilters.Any())
            {
                query = query.Where(p => filterDto.GenderFilters.Contains(p.Gender));
            }

            if (filterDto.BrandFilters.Any())
            {
                query = query.Where(p => filterDto.BrandFilters.Contains(p.Brand.Name));
            }

            if (filterDto.SizeFilters.Any())
            {
                query = query.Where(p => filterDto.SizeFilters.Contains(p.Size));
            }

            Console.WriteLine($"PriceRange.Min: {filterDto.PriceRange.Min}, PriceRange.Max: {filterDto.PriceRange.Max}");

            var minPrice = filterDto.PriceRange.Min > 0 ? filterDto.PriceRange.Min : 0;
            var maxPrice = filterDto.PriceRange.Max < decimal.MaxValue ? filterDto.PriceRange.Max : decimal.MaxValue;

            query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            if (!string.IsNullOrEmpty(filterDto.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(filterDto.SearchTerm) || p.Description.Contains(filterDto.SearchTerm) || p.Brand.Name.Contains(filterDto.SearchTerm));
            }
             


            query = filterDto.SortOption switch
            {
                "az" => query.OrderBy(p => p.Name),
                "za" => query.OrderByDescending(p => p.Name),
                "priceLowHigh" => query.OrderBy(p => p.Price),
                "priceHighLow" => query.OrderByDescending(p => p.Price),
                _ => query
            };

            var totalCount = await query.CountAsync();

            var skip = (filterDto.CurrentPage - 1) * filterDto.ItemsPerPage;
            var take = filterDto.ItemsPerPage;

            
            var products = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();


            return (products, totalCount);
        }

        public async Task<SoldProductContract> GetSoldProductsAsync(Guid productId)
        {
            var soldProducts = await _context.OrderItems
                .Where(oi => oi.ProductId == productId)
                .GroupBy(oi => oi.ProductId)
                .Select(group => new SoldProductContract
                {
                    ProductId = group.Key,
                    TotalSold = group.Sum(oi => oi.Quantity)
                })
                .FirstOrDefaultAsync();

            return soldProducts;
        }

        public async Task AddAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> UpdateDeletedAtAsync(Guid productId, int version, DateTime? deletedAt)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == productId && p.Version == version)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null;  
            }

            product.DeletedAt = deletedAt;
            

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetLatestProductsAsync(int count)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p => p.IsLast == true && p.DeletedAt == null)  
                .OrderByDescending(p => p.CreatedAt)              
                .Take(count)                                     
                .ToListAsync();
        }

        public async Task<bool> LockProductAsync(Guid productId, Guid adminId)
        {
            var product = await GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found.");

            if (product.LockedBy != null)
            {
                return false;  
            }


            var entry = _context.Entry(product);
            if (entry.State == EntityState.Detached)
            {
                _context.Products.Attach(product);   
            }

             
            
            product.LockedBy = adminId;

             

            try
            {
                 
                entry.State = EntityState.Modified;

                
                int affectedRows = await _context.SaveChangesAsync();
                Console.WriteLine($"Changes saved successfully. Rows affected: {affectedRows}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return false;
            }

             
            var updatedProduct = await GetProductByIdAsync(productId);
            Console.WriteLine($"Updated product UpdatedAt: {updatedProduct.UpdatedAt}");

            return true;
        }
        public async Task<bool> UnlockProductAsync(Guid productId, Guid adminId)
        {
            var product = await GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found.");

           

            var entry = _context.Entry(product);
            if (entry.State == EntityState.Detached)
            {
                _context.Products.Attach(product); 
            }

            
            product.LockedBy = null;
               

            try
            {
                entry.State = EntityState.Modified;   

                 
                int affectedRows = await _context.SaveChangesAsync();
                Console.WriteLine($"Changes saved successfully. Rows affected: {affectedRows}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return false;
            }

            // Verify the database update
            var updatedProduct = await GetProductByIdAsync(productId);
            Console.WriteLine($"Updated product LockedBy: {updatedProduct.LockedBy}");

            return true;
        }
    }
}
