using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class UserProfileContract
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; } = null;
        public List<AddressContract> Addresses { get; set; } = new List<AddressContract>();

    }
}
