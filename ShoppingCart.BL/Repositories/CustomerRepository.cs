﻿using Dapper;
using Dapper.Contrib.Extensions;
using ShoppingCart.BL.Models;
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
        internal override string ColumnIdName => "Id";
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

    }
}
