using System;
using System.Collections.Generic;

namespace ShoeStore.Repository.Model;

public partial class Cart
{
    public Guid CartId { get; set; }

    public Guid UserId { get; set; }

    public short IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
