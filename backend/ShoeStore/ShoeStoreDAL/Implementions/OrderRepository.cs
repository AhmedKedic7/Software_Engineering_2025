using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Repository.Interfaces;
using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Implementions
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShoeStoreDbContext _context;

        public OrderRepository(ShoeStoreDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order)
        {
            foreach (var orderItem in order.OrderItems)
            {
                var product = orderItem.Product;

                 
                if (_context.Entry(product).State == EntityState.Detached)
                {
                    _context.Attach(product);  
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        // Get orders for a specific user
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Where(o => o.UserId == userId)
                 .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address).
                ToList();
        }
    }
}
