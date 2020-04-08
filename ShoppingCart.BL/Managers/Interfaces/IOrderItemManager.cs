using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface IOrderItemManager : IManager<OrderItem>
    {
        bool DeleteByOrderId(int[] id);
    }
}
