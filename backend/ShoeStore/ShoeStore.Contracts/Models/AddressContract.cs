using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class AddressContract
{
    public Guid AddressId { get; set; }

    public Guid UserId { get; set; }
    public string AddressLine { get; set; } = null!;
    

}
