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
    public class AddressTypeManager : AssociativeEntityManager<AddressType>, IAddressTypeManager
    {
        public override IAssociativeEntityRepository<AddressType> Repository => new AddressTypeRepository();

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

    }
}
