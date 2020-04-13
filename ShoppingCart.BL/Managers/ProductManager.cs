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
    public class ProductManager : BaseManager<Product>, IProductManager
    {
        public override IRepository<Product> Repository => new ProductRepository();

        public new IList<Product> GetAll()
        {
            return Repository.GetAll();
        }

        public new Product GetById(int id) 
        {
            return Repository.GetById(id);
        }

        public new IList<Product> GetByName(string name) 
        {
            return Repository.GetByName(name);
        }

        public int Insert(Product product) 
        {
            return ((IProductRepository)Repository).Insert(product);
        }

        public new bool Update(Product product) 
        {
            return Repository.Update(product);
        }
        
        public new bool Delete(int[] id) 
        {
            return Repository.Delete(id);
        }
        
    }
}
