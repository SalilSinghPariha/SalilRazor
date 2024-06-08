using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;

namespace SalilWeb.Pages.Admin.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public EditModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }
        public Category Category { get; set; }

        // by default only get handle will be creating but if we want to create or post request then we required post handler
        public void OnGet(int id)
        {
            Category = _unitOfWorks.CategoryRepository.GetFirstOrDefault(x => x.Id == id);
                

        }

        //Now instead of passing properties in OnPost method we can have BindPropery annotation which will map model properties to page
        //and post call as well and we don't need to require to bind or pass to any mehtod since it will take care automatically
        public async Task<IActionResult> OnPost()
        {
            // addAsync to add the change but untill and unless will save async then it won't save so keep in mind
            if(ModelState.IsValid)
            {
                _unitOfWorks.CategoryRepository.Update(Category);
                _unitOfWorks.Save();
                TempData["Success"] = "Category updated Succesfully";
                return RedirectToPage("index");
            }
           return Page();

        }

    }
}
