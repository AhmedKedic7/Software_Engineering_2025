using System;
using System.Collections.Generic;

namespace ShoeStore.Repository.Model;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
