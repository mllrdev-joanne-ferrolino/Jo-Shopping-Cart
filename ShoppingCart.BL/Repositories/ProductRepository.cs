using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShoppingCart.BL.Repositories
{
    internal class ProductRepository : MainEntityRepository<Product>, IProductRepository
    {
        internal override string TableName => "Product";
        public new IList<Product> GetAll()
        {
            return base.GetAll();
        }

        public new Product GetById(int id) 
        {
            return base.GetById(id);
        }

        public new IList<Product> GetByName(string name)
        {
            return base.GetByName(name);
        }

        public new int Insert(Product product)
        {
            return base.Insert(product);
        }

        public new bool Update(Product product) 
        {
            return base.Update(product);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public IList<Product> GetActiveItems() 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE Status = 'active'";
                return _connection.Query<Product>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }




    }
}
