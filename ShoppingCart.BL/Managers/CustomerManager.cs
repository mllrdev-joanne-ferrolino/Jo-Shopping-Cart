﻿using ShoppingCart.BL.Managers.Interfaces;
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
    public class CustomerManager : MainEntityManager<Customer>, ICustomerManager
    {
        public override IMainEntityRepository<Customer> Repository => new CustomerRepository();

        public IList<Customer> GetAll()
        {
            return ((ICustomerRepository)Repository).GetAll();
        }

        public new Customer GetById(int id)
        {
            return Repository.GetById(id);
        }
        public new IList<Customer> GetByName(string name)
        {
            return Repository.GetByName(name);
        }
        public new int Insert(Customer customer)
        {
            return Repository.Insert(customer);
        }

        public new bool Update(Customer customer)
        {
            return Repository.Update(customer);
        }

        public bool Delete(int[] id)
        {
            return ((ICustomerRepository)Repository).Delete(id);
        }

        public new int GetId(int id)
        {
            return Repository.GetId(id);
        }

        public bool ItemExist(string firstName, string lastName) 
        {
            return ((ICustomerRepository)Repository).ItemExist(firstName, lastName);
        }
    }
}
