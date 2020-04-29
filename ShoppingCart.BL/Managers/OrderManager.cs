﻿using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Entities;
using ShoppingCart.BL.Repositories;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public class OrderManager : MainEntityManager<Order>, IOrderManager
    {
        public override IMainEntityRepository<Order> Repository => new OrderRepository();

        public IList<Order> GetAll()
        {
            return ((IOrderRepository)Repository).GetAll();
        }

        public new Order GetById(int id)
        {
            return Repository.GetById(id);
        }
        public new IList<Order> GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public new int Insert(Order order)
        {
            return ((IOrderRepository)Repository).Insert(order);
        }

        public new bool Update(Order order)
        {
            return Repository.Update(order);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }

        public IList<Order> GetByCustomerId(int id) 
        {
            return ((IOrderRepository)Repository).GetByCustomerId(id);
        }

        public new IList<Order> Search(List<string> conditions) 
        {
            return Repository.Search(conditions);
        }
    }
}
