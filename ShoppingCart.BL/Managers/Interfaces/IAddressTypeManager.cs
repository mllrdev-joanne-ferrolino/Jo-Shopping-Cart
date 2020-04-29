using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers.Interfaces
{
    public interface IAddressTypeManager : IManager<AddressType>, IJunctionEntityManager<AddressType>
    {
        AddressType GetAddressType(int id);

        IList<AddressType> GetByCustomerId(int id);

        bool Delete(int[] id);
        AddressType GetByAddressId(int id);
        IList<AddressType> GetByName(string name);
    }
}
