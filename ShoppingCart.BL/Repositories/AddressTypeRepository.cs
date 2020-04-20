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
    internal class AddressTypeRepository : AssociativeEntityRepository<AddressType>, IAddressTypeRepository
    {
        internal override string TableName => "AddressType";
        internal override string ColumnIdName => "CustomerId";
        public new IList<AddressType> GetAll()
        {
            return base.GetAll();
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public new bool Insert(AddressType addressType)
        {
            return base.Insert(addressType);
        }
    }
}
