﻿using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>, IJunctionEntityRepository<OrderItem>
    {
        IList<int> GetByProductId(int id);
        IList<OrderItem> GetByOrderId(int id);

        bool DeleteByProductId(int[] id);

        bool DeleteByOrderId(int[] id);
    }
}
