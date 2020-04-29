using Dapper;
using Dapper.Contrib.Extensions;
using ShoppingCart.BL.Entities;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories
{
    internal class CustomerRepository : MainEntityRepository<Customer>, ICustomerRepository
    {
        internal override string TableName => "Customer";
        public new IList<Customer> GetAll()
        {
            return base.GetAll();
        }

        public new Customer GetById(int id)
        {
            return base.GetById(id);
        }

        public new int GetId(int id)
        {
            return base.GetId(id);
        }

        public new IList<Customer> GetByName(string name) 
        {
            return base.GetByName(name);
        }

        public new int Insert(Customer customer)
        {
            return base.Insert(customer);
        }

        public new bool Update(Customer customer)
        {
            return base.Update(customer);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public bool ItemExist(string firstName, string lastName) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE LastName = '{lastName}' AND FirstName = '{firstName}'";
                return _connection.Query(sql).Count() > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }

        }

        public List<Customer> GetSearchResult(string name) 
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE LOWER(LastName) LIKE '%{name}%' OR LOWER(FirstName) LIKE '%{name}%'";
                return _connection.Query<Customer>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public new IList<Customer> Search(List<string> conditions) 
        {
            return base.Search(conditions);
        }
    }
}
