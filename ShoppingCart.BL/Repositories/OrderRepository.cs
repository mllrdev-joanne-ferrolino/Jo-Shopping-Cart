﻿using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories
{
    internal class OrderRepository : MainEntityRepository<Order>, IOrderRepository
    {
        internal override string TableName => "[Order]";
        internal override string ColumnIdName => "Id";
        public new IList<Order> GetAll()
        {
            return base.GetAll();
        }

        public new Order GetById(int id)
        {
            return base.GetById(id);
        }

        public new IList<Order> GetByName(string name) 
        {
            return base.GetByName(name);
        }

        public new int Insert(Order order)
        {
            return base.Insert(order);
        }

        public new bool Update(Order order)
        {
            return base.Update(order);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

    }
}
