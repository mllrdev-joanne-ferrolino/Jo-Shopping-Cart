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
    public class ProductManager : MainEntityManager<Product>, IProductManager
    {
        public override IMainEntityRepository<Product> Repository => new ProductRepository();

        public IList<Product> GetAll()
        {
            return ((IProductRepository)Repository).GetAll();
        }

        public new Product GetById(int id) 
        {
            return Repository.GetById(id);
        }

        public new IList<Product> GetByName(string name) 
        {
            return Repository.GetByName(name);
        }

        public new int Insert(Product product) 
        {
            return Repository.Insert(product);
        }

        public new bool Update(Product product) 
        {
            return Repository.Update(product);
        }
        
        public new bool Delete(int[] id) 
        {
            return Repository.Delete(id);
        }

        public IList<Product> GetActiveItems() 
        {
            return ((IProductRepository)Repository).GetActiveItems();
        }

        public new IList<Product> Search(Product product) 
        {
            return Repository.Search(product);
        }
    }
}
