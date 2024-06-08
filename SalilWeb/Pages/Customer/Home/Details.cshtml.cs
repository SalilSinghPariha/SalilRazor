using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using Salil.Utility;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SalilWeb.Pages.Customer.Home
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public DetailsModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            
        }

        //public MenuItem menuItems { get; set; }
        //[Range(1,100,ErrorMessage ="Count between 1-100")]
        //public int Count { get; set; }
        [BindProperty]
        public ShoppingCart shoppingCart { get; set; }
        public void OnGet(int id)
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCart = new()
            {
                ApplicationUserId = claim.Value,
                menuItem = _unitOfWorks.menuItemRepository.GetFirstOrDefault(u => u.Id == id, includeProperites: "category,foodType"),
                MenuItemId=id

        };

    }
        public IActionResult OnPost()
        {
            var shoppingCartFromDb = _unitOfWorks.shoppingCartRepository.GetFirstOrDefault(
                filter:u=>u.ApplicationUserId==shoppingCart.ApplicationUserId &&
                u.MenuItemId==shoppingCart.MenuItemId
                );
            if (shoppingCartFromDb == null)
            {
                if (ModelState.IsValid)
                {

                    _unitOfWorks.shoppingCartRepository.Add(shoppingCart);
                    _unitOfWorks.Save();
                    HttpContext.Session.SetInt32(SD.sessionCart, 
                        _unitOfWorks.shoppingCartRepository.GetAll(u=>u.ApplicationUserId==shoppingCart.ApplicationUserId).ToList().Count
                        );
                }
            }
            else
            {
                _unitOfWorks.shoppingCartRepository.IncrementShoppingCart(shoppingCartFromDb, shoppingCart.Count);
            }
            return RedirectToPage("Index");

        }
    }
}
