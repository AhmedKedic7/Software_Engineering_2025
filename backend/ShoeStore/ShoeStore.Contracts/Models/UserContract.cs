using System;
using System.Collections.Generic;

namespace ShoeStore.Contracts.Models;

public class UserContract
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public short IsAdmin { get; set; }

    public short IsLogedin { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<AddressContract> Addresses { get; set; } = new List<AddressContract>();

    public virtual ICollection<CartContract> Carts { get; set; } = new List<CartContract>();

    public virtual ICollection<OrderContract> Orders { get; set; } = new List<OrderContract>();
}
