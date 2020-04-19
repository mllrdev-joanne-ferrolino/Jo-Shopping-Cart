using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Repositories;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public abstract class BaseManager<T> where T: class
    {
        public abstract IRepository<T> Repository { get; }
       
        public IList<T> GetAll() 
        { 
            return Repository.GetAll();
        }
       
        public bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }
    }
}
