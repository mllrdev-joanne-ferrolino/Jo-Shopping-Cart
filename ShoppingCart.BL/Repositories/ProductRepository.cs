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


namespace ShoppingCart.BL.Repositories
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
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

        public new bool Insert(Product product) 
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
    }
}
