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
    public class OrderItemManager : BaseManager<OrderItem>, IOrderItemManager
    {
        public override IRepository<OrderItem> Repository => new OrderItemRepository();
        public new IList<OrderItem> GetAll()
        {
            return Repository.GetAll();
        }

        public new OrderItem GetById(int id)
        {
            return Repository.GetById(id);
        }
        public new IList<OrderItem> GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public new int Insert(OrderItem orderItem)
        {
            return Repository.Insert(orderItem);
        }

        public new bool Update(OrderItem orderItem)
        {
            return Repository.Update(orderItem);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }

        public bool DeleteByOrderId(int[] id) 
        {
            return ((IOrderItemRepository)Repository).DeleteByOrderId(id);
        }
    }
}
