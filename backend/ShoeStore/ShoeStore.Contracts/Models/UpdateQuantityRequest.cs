using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class UpdateQuantityRequest
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
