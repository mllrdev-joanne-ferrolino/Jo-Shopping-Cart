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
    internal class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        internal override string TableName => "Address";
        public new IList<Address> GetAll()
        {
            return base.GetAll();
        }

        public new Address GetById(int id)
        {
            return base.GetById(id);
        }
        public new IList<Address> GetByName(string name)
        {
            return base.GetByName(name);
        }
        public new bool Insert(Address address)
        {
            return base.Insert(address);
        }

        public new bool Update(Address address)
        {
            return base.Update(address);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public int GetId(int id)
        {
            try
            {
                string sql = $"SELECT {id} FROM {TableName}";
                return _connection.Execute(sql);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

    }
}
