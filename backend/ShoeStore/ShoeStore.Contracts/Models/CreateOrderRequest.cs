using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid AddressId { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; }
    }
}
