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
                ColorId = createProductContract.ColorId
            };

            

            

            await _productRepository.AddProductAsync(product);

            return _mapper.Map<ProductContract>(product);
        }
    }

}
