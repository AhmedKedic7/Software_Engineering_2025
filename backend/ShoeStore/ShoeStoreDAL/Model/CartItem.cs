using System;
using System.Collections.Generic;

namespace ShoeStore.Repository.Model;

public partial class CartItem
{
    public Guid CartItemId { get; set; }

    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? Version { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    //public virtual Product? ProductNavigation { get; set; }
}
