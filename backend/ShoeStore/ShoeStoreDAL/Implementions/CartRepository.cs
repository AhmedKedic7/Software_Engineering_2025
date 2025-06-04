using Microsoft.EntityFrameworkCore;
using ShoeStore.Repository.Model;
using ShoeStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Repository.Implementions
{
    public class CartRepository : ICartRepository
    {
        private readonly ShoeStoreDbContext _context;
        private readonly ProductRepository _productRepository;

        public CartRepository(ShoeStoreDbContext context, ProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        public async Task<Cart> GetActiveCartByUserIdAsync(Guid userId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            try
            {
                return await _context.Carts
        .Include(c => c.CartItems)
        .ThenInclude(ci => ci.Product) // Include all products
        .Where(c => c.UserId == userId && c.IsActive == 1) // Filter carts
        .Select(c => new Cart
        {
            CartId = c.CartId,
            UserId = c.UserId,
            IsActive = c.IsActive,
            CartItems = c.CartItems
                .Where(ci => ci.Product.DeletedAt == null && ci.Product.IsLast==true) // Filter products here
                .ToList()
        })
        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
#pragma warning restore CS8603 // Possible null reference return.

        }
        public async Task<Cart> CreateCartAsync(Guid userId)
        {
            var cart = new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = userId,
                IsActive = 1,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            var latestVersion = await _productRepository.GetLatestProductVersionAsync(cartItem.ProductId);

             
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId && p.Version == latestVersion && p.DeletedAt == null);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            
            if (product.QuantityInStock < cartItem.Quantity)
            {
                throw new Exception("Insufficient stock.");
            }
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            await UpdateCartActivityAsync(cartItem.CartId);
        }

        public async Task RemoveCartItemAsync(Guid cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CheckoutAsync(Guid cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart != null)
            {
                foreach (var cartItem in cart.CartItems)
                {
                    var product = cartItem.Product;
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {cartItem.ProductId} not found.");
                    }

                    if (product.QuantityInStock < cartItem.Quantity)
                    {
                        throw new Exception($"Insufficient stock for product {product.ProductId}. Available: {product.QuantityInStock}, Required: {cartItem.Quantity}");
                    }

                    product.QuantityInStock -= cartItem.Quantity;
                }

                cart.IsActive = 0;
                await _context.SaveChangesAsync();

            }
        }
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            var latestVersion = await _productRepository.GetLatestProductVersionAsync(cartItem.ProductId);
            var product = await _context.Products.FindAsync(cartItem.ProductId, latestVersion);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Check if the quantity in stock is sufficient
            if (product.QuantityInStock < cartItem.Quantity)
            {
                throw new Exception($"Insufficient stock for product {product.ProductId}. Available: {product.QuantityInStock}, Required: {cartItem.Quantity}");
            }

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            await UpdateCartActivityAsync(cartItem.CartId);
        }
    
        public async Task<CartItem> GetCartItemByIdAsync(Guid cartItemId)
        {
            return await _context.CartItems.FindAsync(cartItemId);
        }

        public async Task<DateTime?> GetLastCartActivityTimeAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Where(c => c.UserId == userId && c.IsActive == 1)
                .OrderByDescending(c => c.UpdatedAt)  
                .FirstOrDefaultAsync();

            return cart?.UpdatedAt;
        }

        public async Task UpdateCartActivityAsync(Guid cartId)
        {
           
            var cart = await _context.Carts
                .Where(c => c.CartId == cartId && c.IsActive == 1)
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                cart.UpdatedAt = DateTime.UtcNow;  
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateLastNotifiedAsync(Guid userId, DateTime lastNotified)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive==1); 
            if (cart != null)
            {
                cart.DeletedAt = lastNotified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCartEligibleForNotificationAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .Where(c => c.UserId == userId && c.IsActive == 1)
                .FirstOrDefaultAsync();

            return cart != null && cart.CartItems.Any();  
        }

        public async Task<List<Guid>> GetUsersWithInactiveCartsAsync(TimeSpan inactivityThreshold)
        {
            var thresholdTime = DateTime.UtcNow.Subtract(inactivityThreshold);

             
            var inactiveCarts = await _context.Carts
                .Where(cart =>
                    cart.IsActive == 1 &&
                    cart.UpdatedAt <= thresholdTime && (!cart.DeletedAt.HasValue || cart.DeletedAt <= thresholdTime) &&
                    cart.CartItems.Any()  
                )
                .Select(cart => cart.UserId)  
                .Distinct()
                .ToListAsync();

            return inactiveCarts;
        }

    }
}
