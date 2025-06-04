using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class SoldProductContract
    {
        public Guid ProductId { get; set; }
        public int TotalSold { get; set; }
    }
}
