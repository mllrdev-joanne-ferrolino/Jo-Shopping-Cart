using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public class OrderManager : BaseManager<Order>, IOrderManager
    {
        public override IRepository<Order> Repository => new OrderRepository();

        public new IList<Order> GetAll()
        {
            return Repository.GetAll();
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
            return Repository.Insert(order);
        }

        public new bool Update(Order order)
        {
            return Repository.Update(order);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }
    }
}
