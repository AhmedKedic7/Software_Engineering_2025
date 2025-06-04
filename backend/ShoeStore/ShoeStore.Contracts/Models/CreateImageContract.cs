
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStore.Contracts.Models
{
    public class CreateImageContract
    {
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
