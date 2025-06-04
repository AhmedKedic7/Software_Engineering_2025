using Microsoft.AspNetCore.Mvc;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
using ShoeStore.Services.Implementations;
using ShoeStore.Services.Interfaces;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Orders : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;


        public Orders(IOrderService orderService,IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderContract>> GetOrders()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                 
                Console.WriteLine($"CreateOrderRequest received: UserId={request.UserId}, OrderItemsCount={request.OrderItems?.Count}");

                var order = await _orderService.CreateOrderAsync(request);

                 
                Console.WriteLine($"Order created successfully with ID: {order.OrderId}");

                return CreatedAtAction(nameof(GetOrder), new { orderId = order.OrderId }, order);
            }
            catch (ArgumentException ex)
            {
                 
                Console.WriteLine($"BadRequest: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
               
                Console.WriteLine($"NotFound: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                 
                Console.WriteLine($"InternalServerError: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId, Guid userId)
        {
            var order = await _orderService.GetOrderAsync(orderId, userId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                if (orders == null || !orders.Any())
                {
                    return NotFound($"No orders found for user with ID {userId}.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
