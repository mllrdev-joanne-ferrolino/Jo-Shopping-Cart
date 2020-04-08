using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
     public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        T GetById(int id);
        IList<T> GetByName(string name);
        bool Insert(T obj);
        bool Update(T obj);
        bool Delete(int[] id);
    }
}
