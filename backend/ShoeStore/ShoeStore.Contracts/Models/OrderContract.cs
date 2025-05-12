using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class OrderContract
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<OrderItemContract> OrderItems { get; set; } = new List<OrderItemContract>();

    public UserContract User { get; set; } = null!;
}
