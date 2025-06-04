using ShoeStore.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Interfaces
{
    public interface ICartRepository
    {
        
        Task<Cart> GetActiveCartByUserIdAsync(Guid userId);

        Task<Cart> CreateCartAsync(Guid userId);

       
        Task AddCartItemAsync(CartItem cartItem);

        
        Task RemoveCartItemAsync(Guid cartItemId);

        
        Task CheckoutAsync(Guid cartId);


        Task UpdateCartItemAsync(CartItem cartItem);
        Task<CartItem> GetCartItemByIdAsync(Guid cartItemId);

        Task<bool> IsCartEligibleForNotificationAsync(Guid cartId);

        Task<DateTime?> GetLastCartActivityTimeAsync(Guid cartId);
        Task UpdateCartActivityAsync(Guid userId);

        Task<List<Guid>> GetUsersWithInactiveCartsAsync(TimeSpan inactivityThreshold);

        Task UpdateLastNotifiedAsync(Guid userId, DateTime lastNotified);
    }
}
