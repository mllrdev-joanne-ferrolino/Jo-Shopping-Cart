﻿using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface ICustomerManager : IManager<Customer>, IIndependentManager<Customer>
    {
        int GetId(int id);
    }
}
