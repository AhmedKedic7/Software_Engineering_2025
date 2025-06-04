using AutoMapper;
using ShoeStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStore.Repository.Interfaces;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace ShoeStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<ProductContract> GetAllProducts()
        {
            var products = _productRepository.GetAllProducts();
            return _mapper.Map<IEnumerable<ProductContract>>(products);
        }
        public async Task<ProductContract?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductContract>(product);
        }
        public async Task<ProductContract> AddProductAsync(CreateProductContract createProductContract)
        {
            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                BrandId = createProductContract.BrandId,
                Name = createProductContract.Name,
                Gender = createProductContract.Gender,
                Size = createProductContract.Size,
                Description = createProductContract.Description,
                Price = createProductContract.Price,
                CreatedAt = DateTime.UtcNow,
                ColorId = createProductContract.ColorId,
                QuantityInStock = createProductContract.QuantityInStock,
                IsLast = true,
            };

            await _productRepository.AddProductAsync(product);

            return _mapper.Map<ProductContract>(product);
        }

        public async Task<(IEnumerable<ProductContract> Products, int TotalCount)> GetFilteredProductsAsync(ProductFilterContract filterDto)
        {
            var (products, totalCount) = await _productRepository.GetFilteredProductsAsync(filterDto);
            var productContracts = _mapper.Map<IEnumerable<ProductContract>>(products);
            return (productContracts, totalCount);
        }

        public async Task<SoldProductContract> GetSoldProductsAsync(Guid productId )
        {
             
            var soldProducts = await _productRepository.GetSoldProductsAsync(productId);

             
            return soldProducts;
        }

        public async Task AddImageAsync(Image image)
        {
            await _productRepository.AddAsync(image);
        }

        public async Task<Product> CreateNewVersionAsync(UpdateProductContract request)
        {
             
            var latestProductVersion = await _productRepository.GetLatestProductVersionAsync(request.ProductId);

            
            var newVersion = latestProductVersion == null ? 1 : latestProductVersion + 1;

             
            var newProduct = _mapper.Map<Product>(request);
            newProduct.CreatedAt = DateTime.UtcNow;
            newProduct.Version = newVersion;

            
            try
            {
                await _productRepository.AddProductAsync(newProduct);
            }
            catch (Exception ex)
            {
                 
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
            return newProduct;

        }

        public async Task<Product> UpdateDeletedAtAsync(Guid productId, int version, DateTime? deletedAt)
        {
            return await _productRepository.UpdateDeletedAtAsync(productId, version, deletedAt);
        }

        public async Task<List<ProductContract>> GetLatestProductsAsync(int count)
        {
            var products = await _productRepository.GetLatestProductsAsync(count);
            return _mapper.Map<List<ProductContract>>(products); 
        }

        public async Task<bool> LockProductAsync(Guid productId, Guid adminId)
        {
            return await _productRepository.LockProductAsync(productId, adminId);
        }

        public async Task<bool> UnlockProductAsync(Guid productId, Guid adminId)
        {
            return await _productRepository.UnlockProductAsync(productId, adminId);
        }

    }

}
