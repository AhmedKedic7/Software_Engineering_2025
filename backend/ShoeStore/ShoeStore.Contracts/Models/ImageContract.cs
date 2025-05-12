using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class ImageContract
{
    public int ImageId { get; set; }

    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsMain { get; set; }

    public DateTime? CreatedAt { get; set; }

}
