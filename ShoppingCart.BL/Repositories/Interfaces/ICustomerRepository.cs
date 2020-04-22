using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>, IMainEntityRepository<Customer>
    {
        bool ItemExist(string firstName, string lastName);
        List<Customer> GetSearchResult(string name);

    }
}
