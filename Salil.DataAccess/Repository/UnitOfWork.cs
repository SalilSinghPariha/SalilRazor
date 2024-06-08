using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWorks
    {
        public ICategoryRepository CategoryRepository { get;private set; }
        public IFoodTypeRepository foodTypeRepository { get; private set; }
        public IMenuItemRepository menuItemRepository { get; private set; }

        public IShoppingCartRepository shoppingCartRepository { get; private set; }
        public IOrderHeaderRepository orderHeaderRepository { get; private set; }   
        public IOrderDetailsRepository orderDetailsRepository { get; private set; }
        public IApplicationUserRepository applicationUserRepository { get; private set; }

        private readonly ApplicationDBContext _dbContext;

        public UnitOfWork(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;

            CategoryRepository = new CategoryRespository(_dbContext);
            foodTypeRepository = new FoodTypeRepository(_dbContext);
            menuItemRepository = new MenuItemRepository(_dbContext);
            shoppingCartRepository = new ShoppingCartRepository(_dbContext);
            orderDetailsRepository = new OrderDetailsRepository(_dbContext);
            orderHeaderRepository   = new OrderHeaderRepository(_dbContext);
            applicationUserRepository = new ApplicationUserRepository(_dbContext);
        }
        public void Dispose()
        {
           _dbContext.Dispose();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
