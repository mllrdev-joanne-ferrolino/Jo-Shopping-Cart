﻿using Dapper;
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
        public int Insert(Address address)
        {
            try
            {
                var properties = address.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var fields = string.Join(", ", properties.Select(e => e.Name));
                var values = string.Join(", ", properties.Select(e => $"@{e.Name}"));
                string sql = $"INSERT INTO {TableName} ({fields}) VALUES ({values}); SELECT @@IDENTITY";
                return _connection.ExecuteScalar<int>(sql, address);

            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }

        }
    }
}
