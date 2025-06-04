using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoeStore.Services.Interfaces;
using ShoeStore.Contracts.Models;
using System;
using ShoeStore.Repository.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Products : ControllerBase
    {
        private readonly IProductService _productService;


        public Products(IProductService productService, ILogger<Products> logger)
        {
            _productService = productService;

        }
        // GET: api/<Products>
        [HttpGet]
        public ActionResult<IEnumerable<ProductContract>> GetProducts()
        {

            var products = _productService.GetAllProducts();


            return Ok(products);
        }

        [HttpGet("sold-products/{productId}")]
        public async Task<ActionResult<SoldProductContract>> GetSoldProduct(Guid productId)
        {
            var soldProduct = await _productService.GetSoldProductsAsync(productId);
            if (soldProduct == null) return null;
            return Ok(soldProduct);
        }


        // GET api/<Products>/16d945fe-1fda-4854-a0ec-d0a05208d227
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        // POST api/<Products>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductContract createProductContract)
        {
            if (createProductContract == null)
            {
                return BadRequest("Product data is null.");
            }

            int version = 1;

            var product = await _productService.AddProductAsync(createProductContract);

            foreach (var imageDto in createProductContract.Images)
            {
                var image = new Image
                {
                    ProductId = product.ProductId,
                    Version = version,
                    ImageUrl = imageDto.ImageUrl,
                    IsMain = imageDto.IsMain,
                    CreatedAt = DateTime.UtcNow
                };
                await _productService.AddImageAsync(image);
            }
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpPost("update")]
        public async Task<IActionResult> CreateProduct([FromBody] UpdateProductContract request)
        {
            try
            {
                var newProduct = await _productService.CreateNewVersionAsync(request);
                return CreatedAtAction(nameof(CreateProduct), new { id = newProduct.ProductId, version = newProduct.Version }, newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredProducts([FromBody] ProductFilterContract filterDto)
        {
            var (products, totalCount) = await _productService.GetFilteredProductsAsync(filterDto);
            return Ok(new { Products = products, TotalCount = totalCount });
        }


        [HttpPatch("{productId}/{version}")]
        public async Task<IActionResult> UpdateDeletedAt(Guid productId, int version, [FromBody] DateTime? deletedAt)
        {
            //if ()
            //{
            //    return BadRequest("deletedAt value must be provided.");
            //}

            try
            {
                var updatedProduct = await _productService.UpdateDeletedAtAsync(productId, version, deletedAt);
                if (updatedProduct == null)
                {
                    return NotFound("Product not found or version mismatch.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestProducts()
        {
            var products = await _productService.GetLatestProductsAsync(4);
            return Ok(products);
        }

        [HttpPut("{productId}/lock")]
        public async Task<IActionResult> LockProduct(Guid productId, [FromBody] LockRequest lockRequest)
        {
            try
            {
                var result = await _productService.LockProductAsync(productId, lockRequest.AdminId);
                if (!result)
                {
                    return Conflict(new { message = "Product is already locked by another admin." });
                }
                return Ok(new { message = "Product locked successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{productId}/unlock")]
        public async Task<IActionResult> UnlockProduct(Guid productId, [FromBody] LockRequest lockRequest)
        {
            try
            {
                var result = await _productService.UnlockProductAsync(productId, lockRequest.AdminId);
                if (!result)
                {
                    return Conflict(new { message = "You cannot unlock this product." });
                }
                return Ok(new { message = "Product unlocked successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
