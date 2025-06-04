using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Implementions;
using ShoeStore.Repository.Interfaces;
using ShoeStore.Repository.Model;
using ShoeStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductService productService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productService = productService;
        }

        public async Task<OrderContract> CreateOrderAsync(CreateOrderRequest request)
        {
            if (request.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId is required.");
            }

            if (request.OrderItems == null || !request.OrderItems.Any())
            {
                throw new ArgumentException("OrderItems cannot be empty.");
            }

            
            var orderItems = new List<OrderItem>();
            

            foreach (var itemRequest in request.OrderItems)
            {
                var productContract = await _productService.GetProductByIdAsync(itemRequest.ProductId);
                if (productContract == null)
                {
                    throw new KeyNotFoundException($"Product with ID {itemRequest.ProductId} not found.");
                }

                if (productContract.IsLast!=1)
                {
                    throw new InvalidOperationException($"Product with ID {itemRequest.ProductId} is not the latest version.");
                }

                var product = _mapper.Map<Product>(productContract);

                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    ProductId = product.ProductId,
                    Quantity = itemRequest.Quantity,
                    Version = product.Version,  
                    Product = product
                };
                Console.WriteLine(orderItem);

                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = request.UserId,
                TotalPrice = request.TotalPrice,
                CreatedAt = DateTime.UtcNow,
                AddressId = request.AddressId,
                OrderItems = orderItems
            };

            await _orderRepository.CreateOrderAsync(order);

            var orderDto = _mapper.Map<OrderContract>(order);
            orderDto.ContactName = request.ContactName;
            orderDto.Phone = request.Phone;
            orderDto.Email = request.Email;
            orderDto.ShippingAddress = request.ShippingAddress;

            return orderDto;
        }

        public IEnumerable<OrderContract> GetAllOrders()
        {
            var orders = _orderRepository.GetAllOrders();
            return _mapper.Map<IEnumerable<OrderContract>>(orders);
        }

        public async Task<OrderContract> GetOrderAsync(Guid orderId, Guid userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null && order.UserId == userId)
            {
                return _mapper.Map<OrderContract>(order); 
            }
            return null; 
        }

        public async Task<IEnumerable<OrderContract>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderContract>>(orders);  
        }
    }
}
