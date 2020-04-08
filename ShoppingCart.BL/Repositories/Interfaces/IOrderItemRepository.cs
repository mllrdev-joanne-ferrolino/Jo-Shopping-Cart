using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    interface IOrderItemRepository : IRepository<OrderItem>
    {
        bool DeleteByOrderId(int[] id);
    }
}
