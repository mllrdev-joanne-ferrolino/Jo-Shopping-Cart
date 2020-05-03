using Dapper;
using ShoppingCart.BL.Entities;
using ShoppingCart.BL.Repositories.Interfaces;
using ShoppingCart2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShoppingCart.BL.Repositories
{
    internal class AddressTypeRepository : JunctionEntityRepository<AddressType>, IAddressTypeRepository
    {
        internal override string TableName => "AddressType";
        public new IList<AddressType> GetAll()
        {
            return base.GetAll();
        }

        public bool Delete(int[] id)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    string sql = $"DELETE FROM {TableName} WHERE CustomerId IN ({string.Join(", ", id)})";
                    var result = _connection.Execute(sql) > 0;
                    scope.Complete();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }

        public new bool Insert(AddressType addressType)
        {
            return base.Insert(addressType);
        }
        
        public AddressType GetAddressType(int id) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE CustomerId = {id}";
                return _connection.QueryFirstOrDefault<AddressType>(sql);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public IList<AddressType> GetByCustomerId(int id) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE CustomerId = {id}";
                return _connection.Query<AddressType>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public AddressType GetByAddressId(int id) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE AddressId = {id}";
                return _connection.QueryFirstOrDefault<AddressType>(sql);

            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }


        public bool Update(AddressType addressType)
        {
            try
            {
                var properties = addressType.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var values = string.Join(", ", properties.Select(e => $"{e.Name} = @{e.Name}"));
                string sql = $"UPDATE {TableName} SET {values} WHERE AddressId = @AddressId";
                return _connection.Execute(sql, addressType) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }

        }

        public IList<AddressType> GetByCode(int code) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE AddressCode = {code}";
                return _connection.Query<AddressType>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }
    }
}
