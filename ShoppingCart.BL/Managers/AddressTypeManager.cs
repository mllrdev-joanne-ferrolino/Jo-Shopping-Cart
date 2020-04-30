using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Entities;
using ShoppingCart.BL.Repositories;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public class AddressTypeManager : JunctionEntityManager<AddressType>, IAddressTypeManager
    {
        public override IJunctionEntityRepository<AddressType> Repository => new AddressTypeRepository();

        public IList<AddressType> GetAll()
        {
            return ((IAddressTypeRepository)Repository).GetAll();
        }

        public bool Delete(int[] id) 
        {
            return ((IAddressTypeRepository)Repository).Delete(id);
        }
        public new bool Insert(AddressType addressType) 
        {
            return Repository.Insert(addressType);
        }

        public AddressType GetAddressType(int id) 
        {
            return ((IAddressTypeRepository)Repository).GetAddressType(id);
        }

        public IList<AddressType> GetByCustomerId(int id) 
        {
            return ((IAddressTypeRepository)Repository).GetByCustomerId(id);
        }
        public AddressType GetByAddressId(int id) 
        {
            return ((IAddressTypeRepository)Repository).GetByAddressId(id);
        }

        public IList<AddressType> GetByName(string name) 
        {
            return ((IAddressTypeRepository)Repository).GetByName(name);
        }

        public bool Update(AddressType addressType) 
        {
            return ((IAddressTypeRepository)Repository).Update(addressType);
        }
        public IList<AddressType> GetByCode(int code) 
        {
            return ((IAddressTypeRepository)Repository).GetByCode(code);
        }

    }
}
