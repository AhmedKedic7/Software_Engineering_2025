using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class CartContract
{
    public Guid CartId { get; set; }

    public Guid UserId { get; set; }

    public short IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<CartItemContract> CartItems { get; set; } = new List<CartItemContract>();

    public UserContract User { get; set; } = null!;
}
