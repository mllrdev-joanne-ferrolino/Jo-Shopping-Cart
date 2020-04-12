using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories
{
    internal class AddressTypeRepository : BaseRepository<AddressType>, IAddressTypeRepository
    {
        internal override string TableName => "AddressType";
        public new IList<AddressType> GetAll()
        {
            return base.GetAll();
        }

        public new AddressType GetById(int id)
        {
            return base.GetById(id);
        }
        public new IList<AddressType> GetByName(string name)
        {
            return base.GetByName(name);
        }
        public new int Insert(AddressType addressType)
        {
            return base.Insert(addressType);
        }

        public new bool Update(AddressType addressType)
        {
            return base.Update(addressType);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public bool DeleteByCustomerId(int[] id)
        {
            try
            {
                string sql = $"DELETE FROM {TableName} WHERE CustomerId IN ({string.Join(", ", id)})";
                return _connection.Execute(sql) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }
    }
}
