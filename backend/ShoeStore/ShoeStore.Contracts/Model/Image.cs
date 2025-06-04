using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Model;

public partial class Image
{
    public int ImageId { get; set; }

    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsMain { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int Version { get; set; }

    public virtual Product Product { get; set; } = null!;
}
