using System;
using System.Collections.Generic;
using System.Drawing;

namespace ShoeStore.Contracts.Models;

public class ProductContract
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

    public int Version { get; set; }

    public int ColorId { get; set; }
    public int QuantityInStock { get; set; }
    public Guid? LockedBy { get; set; }
    public string BrandName { get; set; } = null!;

    public string ColorName { get; set; } = null!;

    public short IsLast { get; set; }

    public virtual ICollection<ImageContract> Images { get; set; } = new List<ImageContract>();
    
}
