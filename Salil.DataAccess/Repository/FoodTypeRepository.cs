using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public FoodTypeRepository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }
        //public void Save()
        //{
        //    _dbContext.SaveChanges();
        //}

        public void Update(FoodType foodType)
        {
            var ftdFromDb = _dbContext.foodTypes.FirstOrDefault(u => u.Id == foodType.Id);
            if (ftdFromDb != null)
            {
                ftdFromDb.Name = foodType.Name;
                //Save();
            }
        }
    }
}
