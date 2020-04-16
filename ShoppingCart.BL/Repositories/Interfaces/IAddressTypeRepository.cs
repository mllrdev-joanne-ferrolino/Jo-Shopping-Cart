﻿using ShoppingCart.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    public interface IAddressTypeRepository : IRepository<AddressType>, IAssociativeEntityRepository<AddressType>
    {
        //bool DeleteByCustomerId(int[] id);
       
    }
}
