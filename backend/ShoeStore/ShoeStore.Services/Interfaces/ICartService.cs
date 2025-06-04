using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;


namespace ShoeStore.Services.Interfaces
{
    public interface ICartService
    {
        Task RemoveItemFromCartAsync(Guid cartItemId);
        Task AddItemToCartAsync(Guid userId, Guid productId, int quantity);
        Task CheckoutAsync(Guid cartId);
        
        Task<CartContract> GetCartByUserIdAsync(Guid userId);

        Task<CartItemContract> UpdateCartItemQuantityAsync(Guid cartItemId, int quantity);
        Task ProcessCartActivityAsync();
    }
}
