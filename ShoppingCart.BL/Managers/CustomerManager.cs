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
    public class CustomerManager : BaseManager<Customer>, ICustomerManager
    {
        public override IRepository<Customer> Repository => new CustomerRepository();

        public new IList<Customer> GetAll()
        {
            return Repository.GetAll();
        }

        public new Customer GetById(int id)
        {
            return Repository.GetById(id);
        }
        public new IList<Customer> GetByName(string name)
        {
            return Repository.GetByName(name);
        }
        public int Insert(Customer customer)
        {
            return ((ICustomerRepository)Repository).Insert(customer);
        }

        public new bool Update(Customer customer)
        {
            return Repository.Update(customer);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }

        public int GetId(int id)
        {
            return ((ICustomerRepository)Repository).GetId(id);
        }
    }
}
