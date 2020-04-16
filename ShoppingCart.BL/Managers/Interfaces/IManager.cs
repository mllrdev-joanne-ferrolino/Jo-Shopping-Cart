using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface IManager<T> where T : class
    {
        IList<T> GetAll();
        bool Delete(int[] id);
    }
}
