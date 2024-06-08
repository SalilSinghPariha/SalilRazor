using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.Model;
using Salil.DataAccess;
using Salil.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalilWeb.Pages.Admin.MenuItems
{
    [BindProperties]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UpsertModel(IUnitOfWorks unitOfWorks, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWorks = unitOfWorks;
            menuItem = new();
            _webHostEnvironment = webHostEnvironment;

        }
        public MenuItem menuItem { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }


        // by default only get handle will be creating but if we want to create or post request then we required post handler
        public void OnGet(int ?id)
        {
            if(id!=null)
            {
                menuItem = _unitOfWorks.menuItemRepository.GetFirstOrDefault(u => u.Id == id);
            }
            CategoryList = _unitOfWorks.CategoryRepository.GetAll().Select(x=>new SelectListItem()
                { 
                Text= x.Name,
                Value=x.Id.ToString(),
            });

            FoodTypeList = _unitOfWorks.foodTypeRepository.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
        }

        //Now instead of passing properties in OnPost method we can have BindPropery annotation which will map model properties to page
        //and post call as well and we don't need to require to bind or pass to any mehtod since it will take care automatically
        public async Task<IActionResult> OnPost()
        {
            string webRoot= _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (menuItem.Id == 0)
            {
                //Create

                string fileName_new= Guid.NewGuid().ToString();

                var upload=Path.Combine(webRoot, @"images\MenuItems");
                var extension= Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload,fileName_new+extension),FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuItem.Image= @"\images\MenuItems\"+fileName_new+extension;
                _unitOfWorks.menuItemRepository.Add(menuItem);
                _unitOfWorks.Save();
                return RedirectToPage("index");
            }

            else
            {
                //Update
                var objFromDb = _unitOfWorks.menuItemRepository.GetFirstOrDefault(u=>u.Id==menuItem.Id);

                if (files.Count > 0)
                {
                    string fileName_new = Guid.NewGuid().ToString();

                    var upload = Path.Combine(webRoot, @"images\MenuItems");
                    var extension = Path.GetExtension(files[0].FileName);

                    // if files count is freater than 0 then we need to delete old image 

                    var deleteImageOldDb = Path.Combine(webRoot, objFromDb.Image.TrimStart('\\'));

                    if (System.IO.File.Exists(deleteImageOldDb))
                    {
                        System.IO.File.Delete(deleteImageOldDb);
                    }

                    // New Upload
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName_new + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    menuItem.Image = @"\images\MenuItems\" + fileName_new + extension;
                }
                else 
                { 
                    menuItem.Image=objFromDb.Image;
                }
                _unitOfWorks.menuItemRepository.Update(menuItem);
                _unitOfWorks.Save();

                return RedirectToPage("index");
            }
            // addAsync to add the change but untill and unless will save async then it won't save so keep in mind
            //if(ModelState.IsValid)
            //{
            //    _unitOfWorks.menuItemRepository.Add(menuItem);
            //    _unitOfWorks.Save();
            //    TempData["Success"] = "Menu Item added Succesfully";
            //    return RedirectToPage("index");
            //}
           return Page();

        }

    }
}
