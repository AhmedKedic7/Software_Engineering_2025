using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Model;

public partial class Product
{
    public Guid ProductId { get; set; }

    public int BrandId { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int Size { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int ColorId { get; set; }

    public int? QuantityInStock { get; set; }

    public int Version { get; set; }

    public bool IsLast { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Color Color { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
