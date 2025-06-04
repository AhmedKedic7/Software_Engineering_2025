using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class CartItemContract
{
    public Guid CartItemId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public ProductContract Product { get; set; }
}
