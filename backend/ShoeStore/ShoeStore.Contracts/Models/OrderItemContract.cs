using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class OrderItemContract
{
    public Guid OrderItemId { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set;  }

    public ProductContract Product { get; set; }


}
