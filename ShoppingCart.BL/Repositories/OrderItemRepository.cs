using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public IList<int> GetByProductId(int id) 
        {
            try
            {
                string sql = $"SELECT ProductId FROM {TableName} WHERE ProductId = {id}";
                return _connection.Query<int>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public IList<OrderItem> GetByOrderId(int id) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE OrderId = {id}";
                return _connection.Query<OrderItem>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }
        public bool DeleteByOrderId(int[] id)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    string sql = $"DELETE FROM {TableName} WHERE OrderId IN ({string.Join(", ", id)})";
                    var result = _connection.Execute(sql) > 0;
                    scope.Complete();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        public bool DeleteByProductId(int[] id)  
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    string sql = $"DELETE FROM {TableName} WHERE ProductId IN ({string.Join(", ", id)})";
                    var result = _connection.Execute(sql) > 0;
                    scope.Complete();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
    }
}
