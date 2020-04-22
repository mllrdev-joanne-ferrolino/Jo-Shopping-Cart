using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public abstract class MainEntityManager<T> where T: class
    {
        public abstract IMainEntityRepository<T> Repository { get; }

        public T GetById(int id)
        {
            return Repository.GetById(id);
        }

        public IList<T> GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public int Insert(T entity)
        {
            return Repository.Insert(entity);
        }

        public bool Update(T entity)
        {
            return Repository.Update(entity);
        }

        public bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }

        public int GetId(int id) 
        {
            return Repository.GetId(id);
        }
    }
}
