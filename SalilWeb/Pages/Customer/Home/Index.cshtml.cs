using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;

namespace SalilWeb.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public IEnumerable<MenuItem> menuItems { get; set; }

        public IEnumerable<Category> categoryList { get; set; }
        public IndexModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            
        }
        public void OnGet()
        {
            menuItems=_unitOfWorks.menuItemRepository.GetAll(includeProperites:"category,foodType");
            categoryList = _unitOfWorks.CategoryRepository.GetAll(orderBy:u=>u.OrderBy(c=>c.DisplayOrder));

        }
    }
}
