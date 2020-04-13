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
    internal class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        internal override string TableName => "[Order]";
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


        public new bool Update(Order order)
        {
            return base.Update(order);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public int Insert(Order order)
        {
            try
            {
                var properties = order.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var fields = string.Join(", ", properties.Select(e => e.Name));
                var values = string.Join(", ", properties.Select(e => $"@{e.Name}"));
                string sql = $"INSERT INTO {TableName} ({fields}) VALUES ({values}); SELECT @@IDENTITY";
                return _connection.ExecuteScalar<int>(sql, order);

            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }

        }
    }
}
