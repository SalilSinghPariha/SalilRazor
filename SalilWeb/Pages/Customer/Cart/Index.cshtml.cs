using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using Salil.Utility;
using System.Security.Claims;

namespace SalilWeb.Pages.Customer.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public IndexModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;

        }
        public IEnumerable<ShoppingCart> ShoppingCartLists { get; set; }
        public double CartTotal { get; set; }
        public void OnGet()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                ShoppingCartLists = _unitOfWorks.shoppingCartRepository.GetAll(
                    filter:u=>u.ApplicationUserId == claim.Value,
                    includeProperites:"menuItem,menuItem.category,menuItem.foodType"
                    );

                foreach (var cartitem in ShoppingCartLists)
                {
                    CartTotal += (cartitem.menuItem.Price* cartitem.Count);
                }
               
            }
        }

     public IActionResult OnPostPlus(int CartId)
        {
            var cart = _unitOfWorks.shoppingCartRepository.GetFirstOrDefault(
                u => u.Id == CartId
                );
            if (cart != null)
            {
                _unitOfWorks.shoppingCartRepository.IncrementShoppingCart(cart, 1);
            }

            return RedirectToPage("/Customer/Cart/Index");
        }
        public IActionResult OnPostMinus(int CartId)
        {
            var cart = _unitOfWorks.shoppingCartRepository.GetFirstOrDefault(
                u => u.Id == CartId
                );
            if (cart.Count == 1)
            {

                //get the count and decrement by 1 
                var count = _unitOfWorks.shoppingCartRepository.GetAll(u => u.ApplicationUserId ==
           cart.ApplicationUserId).ToList().Count - 1;
                _unitOfWorks.shoppingCartRepository.Remove(cart);
                _unitOfWorks.Save();

                //after save update the session back to decerease count
                HttpContext.Session.SetInt32(SD.sessionCart, count);
            }
            else
            {
                _unitOfWorks.shoppingCartRepository.DecrementShoppingCart(cart, 1);
            }

            return RedirectToPage("/Customer/Cart/Index");
        }
        public IActionResult OnPostRemove(int CartId)
        {
            var cart = _unitOfWorks.shoppingCartRepository.GetFirstOrDefault(u => u.Id == CartId);

            //get the count and decrement by 1 
            var count=_unitOfWorks.shoppingCartRepository.GetAll(u => u.ApplicationUserId ==
            cart.ApplicationUserId).ToList().Count - 1;
                       

            _unitOfWorks.shoppingCartRepository.Remove(cart);
                _unitOfWorks.Save();
            //after save update the session back to decerease count
            HttpContext.Session.SetInt32(SD.sessionCart,count);
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
