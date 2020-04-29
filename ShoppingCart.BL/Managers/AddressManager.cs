﻿using ShoppingCart.BL.Managers.Interfaces;
using ShoppingCart.BL.Entities;
using ShoppingCart.BL.Repositories;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public class AddressManager : MainEntityManager<Address>, IAddressManager 
    {
        public override IMainEntityRepository<Address> Repository => new AddressRepository();

        public IList<Address> GetAll()
        {
            return ((IAddressRepository)Repository).GetAll();
        }

        public new Address GetById(int id)
        {
            return Repository.GetById(id);
        }

        public new IList<Address> GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public new int Insert(Address address)
        {
            return Repository.Insert(address);
        }

        public new bool Update(Address address)
        {
            return Repository.Update(address);
        }

        public new bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }
        public new int GetId(int id) 
        {
            return Repository.GetId(id);
        }

        public IList<Address> GetByAddressId(int id) 
        {
            return ((IAddressRepository)Repository).GetByAddressId(id);
        }

        public new IList<Address> Search(List<string> conditions) 
        {
            return Repository.Search(conditions);
        }
    }
}
