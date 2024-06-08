using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;

namespace SalilWeb.Pages.Admin.FoodTypes
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public DeleteModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }
        public FoodType foodType { get; set; }

        // by default only get handle will be creating but if we want to create or post request then we required post handler
        public void OnGet(int id)
        {
            foodType = _unitOfWorks.foodTypeRepository.GetFirstOrDefault(x => x.Id == id);  
                

        }

        //Now instead of passing properties in OnPost method we can have BindPropery annotation which will map model properties to page
        //and post call as well and we don't need to require to bind or pass to any mehtod since it will take care automatically
        public async Task<IActionResult> OnPost()
        {
            
         
                var fdt= _unitOfWorks.foodTypeRepository.GetFirstOrDefault(x => x.Id == foodType.Id);
            if (fdt != null)
                {
                  _unitOfWorks.foodTypeRepository.Remove(fdt);
                  _unitOfWorks.Save();
                  TempData["Success"] = "Food Type deleted Succesfully";
                  return RedirectToPage("index");

            }

            return Page();

        }

    }
}
