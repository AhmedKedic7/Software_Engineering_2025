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
    public Guid? AddressId { get; set; }
    public string ContactName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ShippingAddress { get; set; }
    public List<OrderItemContract> OrderItems { get; set; } = new List<OrderItemContract>();
}
