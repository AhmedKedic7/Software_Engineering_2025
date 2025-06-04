using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
using ShoeStore.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Carts : ControllerBase
    {
        private readonly ICartService _cartService;

        public Carts(ICartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }
        private Guid GetUserIdFromRequest(AddItemToCartRequest request)
        {
            // If you have a UserId in the request body, you can access it here.
            if (request == null || request.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId is required in the request.");
            }

            return request.UserId;
        }


        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartRequest request)
        {
            
            try
            {
               
                await _cartService.AddItemToCartAsync(request.UserId, request.ProductId, request.Quantity);
                await _cartService.ProcessCartActivityAsync();
                return NoContent();  
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Remove an item from the cart
        [HttpDelete("remove-item/{cartItemId}")]
        public async Task<IActionResult> RemoveItemFromCart(Guid cartItemId)
        {
            try
            {
                await _cartService.RemoveItemFromCartAsync(cartItemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Checkout the cart
        [HttpPost("checkout/{cartId}")]
        public async Task<IActionResult> Checkout(Guid cartId)
        {
            try
            {
                await _cartService.CheckoutAsync(cartId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Get cart details by userId
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            try
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return NotFound("Cart not found.");
                }
                 
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-item-quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateQuantityRequest request)
        {
            try
            {
                await _cartService.UpdateCartItemQuantityAsync(request.CartItemId, request.Quantity);

               

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
