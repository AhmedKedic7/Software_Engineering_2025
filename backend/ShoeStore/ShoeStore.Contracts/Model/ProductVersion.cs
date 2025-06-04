using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Model;

public partial class ProductVersion
{
    public int VersionId { get; set; }

    public Guid ProductId { get; set; }

    public short IsLast { get; set; }

    public int Version { get; set; }

    public virtual Product Product { get; set; } = null!;
}
