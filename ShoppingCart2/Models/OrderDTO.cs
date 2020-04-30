using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart2.Models
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public float TotalAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
    }
}
