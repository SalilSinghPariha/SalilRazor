using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ShoppingCartRepository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }

        public int DecrementShoppingCart(ShoppingCart shoppingCart, int count)
        {
            //decrement the count 
            shoppingCart.Count -= count;
            //after that save the changes 
            _dbContext.SaveChanges();
            return shoppingCart.Count;
        }

        public int IncrementShoppingCart(ShoppingCart shoppingCart, int count)
        {
            //Increment the count 
            shoppingCart.Count += count;
            //after that save the changes 
            _dbContext.SaveChanges();
            return shoppingCart.Count;
        }

        //public void Save()
        //{
        //    _dbContext.SaveChanges();
        //}

        public void Update(ShoppingCart shoppingCart)
        {
            var shoppingFromDb = _dbContext.shoppingCarts.FirstOrDefault(u => u.Id == shoppingCart.Id);
            if (shoppingFromDb != null)
            {
                shoppingFromDb.menuItem = shoppingCart.menuItem;
                shoppingFromDb.applicationUser = shoppingCart.applicationUser;
                //Save();
            }
        }
    }
}
