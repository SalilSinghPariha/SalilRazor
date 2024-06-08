using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.Model;
using Salil.DataAccess;
using Salil.DataAccess.Repository.IRepository;

namespace SalilWeb.Pages.Admin.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public CreateModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }
        public Category Category { get; set; }

        // by default only get handle will be creating but if we want to create or post request then we required post handler
        public void OnGet()
        {

        }

        //Now instead of passing properties in OnPost method we can have BindPropery annotation which will map model properties to page
        //and post call as well and we don't need to require to bind or pass to any mehtod since it will take care automatically
        public async Task<IActionResult> OnPost()
        {
            if (Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Category.Name", "Category name and display order can not be same");
            }
            // addAsync to add the change but untill and unless will save async then it won't save so keep in mind
            if(ModelState.IsValid)
            {
                _unitOfWorks.CategoryRepository.Add(Category);
                _unitOfWorks.Save();
                TempData["Success"] = "Category added Succesfully";
                return RedirectToPage("index");
            }
           return Page();

        }

    }
}
