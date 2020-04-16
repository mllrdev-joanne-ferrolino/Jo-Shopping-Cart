using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    public interface IMainEntityRepository<T> where T: class
    {
        int GetId(int id);
        T GetById(int id);
        IList<T> GetByName(string name);
        int Insert(T entity);
        bool Update(T entity);

    }
}
