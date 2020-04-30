using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart2.Models
{
    public class AddressDTO
    {
        public Address Details { get; set; }
        public string Name { get; set; }
        public AddressCode AddressCode { get; set; }
    }
}
