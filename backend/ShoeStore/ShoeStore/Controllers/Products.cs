using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoeStore.Services.Interfaces;
using ShoeStore.Contracts.Models;

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
            // Fetching products from the database
            var products = _productService.GetAllProducts();

            // Returning the products as a JSON response
            return Ok(products);
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

            var product = await _productService.AddProductAsync(createProductContract);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        // PUT api/<Products>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Products>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
