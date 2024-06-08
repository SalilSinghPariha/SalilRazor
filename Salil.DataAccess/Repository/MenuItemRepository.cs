using Salil.DataAccess.Repository.IRepository;
using Salil.Model;

namespace Salil.DataAccess.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MenuItemRepository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }
        //public void Save()
        //{
        //    _dbContext.SaveChanges();
        //}

        public void Update(MenuItem menuItem)
        {
            var menuFromDb = _dbContext.menuItems.FirstOrDefault(u => u.Id == menuItem.Id);
            if (menuFromDb != null)
            {
                menuFromDb.Name = menuItem.Name;
                menuFromDb.Description = menuItem.Description;
                //image property only be updated if it's not null on ideal solution; same applicabel if it's requirment specification
                if(menuFromDb.Image!=null)
                {
                    menuFromDb.Image = menuItem.Image;
                }
                menuFromDb.Price= menuItem.Price;
                menuFromDb.CategoryId = menuItem.CategoryId;
                menuFromDb.FoodTypeId = menuItem.FoodTypeId;
                
            }
        }
    }
}
