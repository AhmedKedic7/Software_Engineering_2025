using System;
using System.Collections.Generic;

namespace ShoeStore.Repository.Model;

public partial class Address
{
    public Guid AddressId { get; set; }

    public Guid UserId { get; set; }

    public string AddressLine { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
