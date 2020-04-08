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
    public class AddressManager : BaseManager<Address>, IAddressManager 
    {
        public override IRepository<Address> Repository => new AddressRepository();

        public new IList<Address> GetAll()
        {
            return Repository.GetAll();
        }

        public new Address GetById(int id)
        {
            return Repository.GetById(id);
        }

        public new IList<Address> GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public new bool Insert(Address address)
        {
            return Repository.Insert(address);
        }

        public new bool Update(Address address)
        {
            return Repository.Update(address);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }
        public int GetId(int id) 
        {
            return ((IAddressRepository)Repository).GetId(id);
        }
    }
}
