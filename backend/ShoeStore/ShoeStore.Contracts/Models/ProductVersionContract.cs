using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class ProductVersionContract
{

    public int VersionId { get; set; }

    public Guid ProductId { get; set; }

    public short IsLast { get; set; }

    public int Version { get; set; }
    

}
