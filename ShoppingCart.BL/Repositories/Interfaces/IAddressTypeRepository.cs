using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    public interface IAddressTypeRepository : IRepository<AddressType>, IJunctionEntityRepository<AddressType>
    {
        AddressType GetAddressType(int id);
        IList<AddressType> GetByCustomerId(int id);
        bool Delete(int[] id);
        AddressType GetByAddressId(int id);
        IList<AddressType> GetByName(string name);
    }
}
