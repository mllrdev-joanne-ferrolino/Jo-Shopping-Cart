using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart2.Models
{
    public class SearchResultDTO
    {
        public Customer Details { get; set; }
        public string FullAddress { get; set; }
        public AddressCode Code { get; set; }
    }
}
