using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository
{
    public class CategoryRespository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CategoryRespository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }
        //public void Save()
        //{
        //    _dbContext.SaveChanges();
        //}

        public void Update(Category category)
        {
            var ctgFromDb = _dbContext.categories.FirstOrDefault(u => u.Id == category.Id);
            if (ctgFromDb != null)
            {
                ctgFromDb.Name = category.Name;
                ctgFromDb.DisplayOrder = category.DisplayOrder;
                //Save();
            }
        }
    }
}
