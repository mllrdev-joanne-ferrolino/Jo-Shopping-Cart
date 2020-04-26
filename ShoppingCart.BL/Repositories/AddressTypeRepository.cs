using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
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

        public IList<AddressType> GetByName(string name) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE Name = '{name}'";
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
