using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class CartContract
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<CartItemContract> CartItems { get; set; } = new List<CartItemContract>();
}
