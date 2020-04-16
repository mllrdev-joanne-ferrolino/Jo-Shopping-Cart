using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories
{
    internal class OrderItemRepository : AssociativeEntityRepository<OrderItem>, IOrderItemRepository
    {
        internal override string TableName => "OrderItem";
        internal override string ColumnIdName => "OrderId";
        public new IList<OrderItem> GetAll()
        {
            return base.GetAll();
        }

        public new bool Insert(OrderItem orderItem)
        {
            return base.Insert(orderItem);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        //public bool DeleteByOrderId(int[] id)
        //{
        //    try
        //    {
        //        string sql = $"DELETE FROM {TableName} WHERE OrderId IN ({string.Join(", ", id)})";
        //        return _connection.Execute(sql) > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex.StackTrace);
        //        return false;
        //    }
        //}

    }
}
