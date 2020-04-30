using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart2.Models
{
    public class CustomerDTO
    {
        public Customer Details { get; set; }
        public List<OrderDTO> Orders { get; set; }
        public List<AddressDTO> Addresses { get; set; }
    }
}
