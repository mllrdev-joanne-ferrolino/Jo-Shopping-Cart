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

        public bool Delete(int[] id)
        {
            return ((IOrderItemRepository)Repository).Delete(id);
        }

    }
}
