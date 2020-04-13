﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface IAssociativeManager<T> where T: class
    {
        bool Insert(T entity);
    }
}
