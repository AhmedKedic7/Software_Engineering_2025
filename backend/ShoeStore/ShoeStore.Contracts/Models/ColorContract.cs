using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class ColorContract
{
    public int ColorId { get; set; }

    public string Name { get; set; } = null!;
}
