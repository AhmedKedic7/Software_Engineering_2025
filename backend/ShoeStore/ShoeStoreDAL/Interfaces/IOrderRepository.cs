using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);  
        Task<Order> GetOrderByIdAsync(Guid orderId);  
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        IEnumerable<Order> GetAllOrders();
    }
}
