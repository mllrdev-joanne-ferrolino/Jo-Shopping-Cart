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
    public class OrderItemManager : AssociativeEntityManager<OrderItem>, IOrderItemManager
    {
        public override IAssociativeEntityRepository<OrderItem> Repository => new OrderItemRepository();
        public IList<OrderItem> GetAll()
        {
            return ((IOrderItemRepository)Repository).GetAll();
        }

        public new bool Insert(OrderItem orderItem)
        {
            return Repository.Insert(orderItem);
        }

        public IList<int> GetByProductId(int id) 
        {
            return ((IOrderItemRepository)Repository).GetByProductId(id);
        }

        public IList<OrderItem> GetByOrderId(int id) 
        {
            return ((IOrderItemRepository)Repository).GetByOrderId(id);
        }

        public bool DeleteByProductId(int[] id) 
        {
            return ((IOrderItemRepository)Repository).DeleteByProductId(id);
        }

        public bool DeleteByOrderId(int[] id)
        {
            return ((IOrderItemRepository)Repository).DeleteByOrderId(id);
        }

    }
}
