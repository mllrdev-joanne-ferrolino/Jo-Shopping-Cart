﻿using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public abstract class JunctionEntityManager<T> where T:class
    {
        public abstract IJunctionEntityRepository<T> Repository { get; }

        public bool Insert(T entity)
        {
            return Repository.Insert(entity);
        }
    }
}
