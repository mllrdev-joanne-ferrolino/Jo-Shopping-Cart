using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Entities
{
    public class AddressType
    {
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; }
        public int AddressCode { get; set; }

    }
}
