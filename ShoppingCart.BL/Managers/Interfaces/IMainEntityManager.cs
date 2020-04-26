using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface IMainEntityManager<T> where T: class
    {
        int GetId(int id);
        T GetById(int id);
        IList<T> GetByName(string name);
        int Insert(T obj);
        bool Update(T obj);
        bool Delete(int[] id);

        IList<T> Search(T obj);
    }
}
