using AutoMapper;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper, IUserRepository userRepository, IEmailService emailService)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task AddItemToCartAsync(Guid userId, Guid productId, int quantity)
        {
            
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);

           
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }

            var latestVersion = await _productRepository.GetLatestProductVersionAsync(productId);

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingCartItem != null)
            {
                
                existingCartItem.Quantity += quantity;
                await _cartRepository.UpdateCartItemAsync(existingCartItem);
            }
            else
            {
               
                var cartItem = new CartItem
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow,
                    Version=latestVersion
                };

                await _cartRepository.AddCartItemAsync(cartItem);
            }
        }

        public async Task RemoveItemFromCartAsync(Guid cartItemId)
        {
            await _cartRepository.RemoveCartItemAsync(cartItemId);
        }

        public async Task CheckoutAsync(Guid cartId)
        {
            
            await _cartRepository.CheckoutAsync(cartId);
        }

        public async Task<CartContract> GetCartByUserIdAsync(Guid userId)
        {
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
            if (cart == null)
            {
                return null;
            }
            foreach (var cartItem in cart.CartItems)
            {
                var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
                cartItem.Product = product;
            }

            var cartContract = _mapper.Map<CartContract>(cart);
            return cartContract;
        }
        public async Task<CartItemContract> UpdateCartItemQuantityAsync(Guid cartItemId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found.");
            }

            cartItem.Quantity = quantity;
            await _cartRepository.UpdateCartItemAsync(cartItem);

            
            var cartItemContract = _mapper.Map<CartItemContract>(cartItem);
            
            return cartItemContract;
        }

        public async Task ProcessCartActivityAsync()
        {

            var inactivityThreshold = TimeSpan.FromMinutes(5);
            var inactiveUserIds = await _cartRepository.GetUsersWithInactiveCartsAsync(inactivityThreshold);

            foreach(var userId in inactiveUserIds)
            {
                var user = _userRepository.GetUserById(userId);
                if(user!=null && !string.IsNullOrEmpty(user.Email))
                {
                    var subject = "Your cart is waiting!";
                    var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #121212;
            color: #ffffff;
            padding: 20px;
            line-height: 1.6;
        }}
        .email-container {{
            background-color: #333333;
            padding: 20px;
            border-radius: 8px;
            max-width: 600px;
            margin: 0 auto;
        }}
        .product {{
            margin-bottom: 15px;
        }}
        .product-title {{
            font-weight: bold;
            font-size: 16px;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            font-size: 16px;
            color: #ffffff;
            background-color: #007bff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #aaaaaa;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <p>Hi {user.FirstName}, we noticed you have some items waiting in your cart</p>
<p> Don't miss out! Checkout now before they're gone.</p>
 

        <div class='footer'>
            If you have any questions, feel free to contact our support team.
             xShoes Team
        </div>
    </div>
</body>
</html>";
                    //var body = $"Hi {user.FirstName},\n\nYou have items in your cart. Don't miss out! Checkout now before they're gone.\n\nBest regards,\nxShoes Team";
                    await _emailService.SendEmailAsync(user.Email, subject, body);

                    await _cartRepository.UpdateLastNotifiedAsync(userId, DateTime.UtcNow);

                    Console.WriteLine($"Email sent to {user.Email} for inactive cart.");
                }
            }
            //await _cartRepository.UpdateCartActivityAsync(userId); // Update activity time

            //var isEligible = await _cartRepository.IsCartEligibleForNotificationAsync(userId);
            //if (isEligible)
            //{
            //    var lastActivity = await _cartRepository.GetLastCartActivityTimeAsync(userId);
            //    if (lastActivity.HasValue)
            //    {
            //        var delay = DateTime.UtcNow.Subtract(lastActivity.Value).TotalMinutes;
            //        if (delay >= 2)
            //        {
            //            var user =  _userRepository.GetUserById(userId);
            //            if (user != null)
            //            {
            //                Console.WriteLine($"Last activity: {lastActivity.Value}, Current time: {DateTime.UtcNow}, Delay: {delay}");
            //                Console.WriteLine(user.Email);
            //                await _emailService.SendEmailAsync(user.Email, "Your cart is waiting!", "You have items in your cart. Checkout now!");



            //            }
            //        }
            //    }
            //}
        }
    }
    


}
