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
    public class AddressTypeManager : BaseManager<AddressType>, IAddressTypeManager
    {
        public override IRepository<AddressType> Repository => new AddressTypeRepository();
        private IAddressTypeRepository _addressTypeRepository = new AddressTypeRepository();

        public new IList<AddressType> GetAll()
        {
            return Repository.GetAll();
        }

        public new AddressType GetById(int id)
        {
            return Repository.GetById(id);
        }
        public new IList<AddressType> GetByName(string name)
        {
            return Repository.GetByName(name);
        }
        public new bool Insert(AddressType addressType)
        {
            return Repository.Insert(addressType);
        }

        public new bool Update(AddressType addressType)
        {
            return Repository.Update(addressType);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }

        public bool DeleteByCustomerId(int[] id) 
        {
            return _addressTypeRepository.DeleteByCustomerId(id);
        }
    }
}
