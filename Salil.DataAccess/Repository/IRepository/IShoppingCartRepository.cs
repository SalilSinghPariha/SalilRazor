using Salil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncrementShoppingCart(ShoppingCart shoppingCart,int count);
        int DecrementShoppingCart(ShoppingCart shoppingCart, int count);

        //void Save();
    }
}
