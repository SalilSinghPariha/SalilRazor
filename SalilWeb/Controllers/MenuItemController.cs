using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Salil.DataAccess.Repository.IRepository;

namespace SalilWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IWebHostEnvironment _environment;
        public MenuItemController(IUnitOfWorks unitOfWorks, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWorks = unitOfWorks;
            _environment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Return all menu list item
            var menuItemsList = _unitOfWorks.menuItemRepository.GetAll(includeProperites:"foodType,category");

            // then return items as json

            return Json(new { data =menuItemsList});
        }
        //id is required so we need to make this id  explicitly as parameter so once will call then it will pass and run the API
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Return menu list item based on id
            var objFromMenuItemDb = _unitOfWorks.menuItemRepository.GetFirstOrDefault(u=>u.Id==id);

            // we need to delete image 

            var deleteImage = Path.Combine(_environment.WebRootPath, objFromMenuItemDb.Image.TrimStart('\\'));

            if (System.IO.File.Exists(deleteImage))
            {
                System.IO.File.Delete(deleteImage);
            }

            // remove and save it
            _unitOfWorks.menuItemRepository.Remove(objFromMenuItemDb);
            _unitOfWorks.Save();

            // then return success message as json
            return Json(new { success="true", message="Deleted Successfully"});
        }
    }
}
