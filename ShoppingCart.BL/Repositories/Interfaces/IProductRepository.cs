using ShoppingCart.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories.Interfaces
{
    public interface IProductRepository: IRepository<Product>, IMainEntityRepository<Product>
    {
        IList<Product> GetActiveItems();
    }
}
