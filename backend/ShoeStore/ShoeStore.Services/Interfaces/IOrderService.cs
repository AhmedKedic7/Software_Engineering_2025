using ShoeStore.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderContract> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderContract> GetOrderAsync(Guid orderId, Guid userId);  
        Task<IEnumerable<OrderContract>> GetOrdersByUserIdAsync(Guid userId);
        IEnumerable<OrderContract> GetAllOrders();
    }
}
