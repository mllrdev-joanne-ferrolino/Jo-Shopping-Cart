﻿using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface IProductManager : IManager<Product>, IMainEntityManager<Product>
    {
        IList<Product> GetActiveItems();
    }
}
