using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface ICustomerManager : IManager<Customer>, IMainEntityManager<Customer>
    {
        bool ItemExist(string firstName, string lastName);
        List<Customer> GetSearchResult(string name);
    }
}
