using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class CartItemContract
{
    public Guid CartItemId { get; set; }

    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public CartContract Cart { get; set; } = null!;

    public ProductContract Product { get; set; } = null!;
}
