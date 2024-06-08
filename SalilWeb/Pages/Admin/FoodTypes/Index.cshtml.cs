using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;

namespace SalilWeb.Pages.Admin.FoodTypes
{
    public class IndexModel : PageModel
    {
        //ready only property for dbcontext which will use dependency injection to access db context proprerty through constructor
        private readonly IUnitOfWorks _unitOfWorks;

        // to fetch all category we can have list of IEnumerable of category

        public IEnumerable<FoodType> foodType { get; set; }
        public IndexModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }

        public void OnGet()
        {
            foodType = _unitOfWorks.foodTypeRepository.GetAll();
        }
    }
}