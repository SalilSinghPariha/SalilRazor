using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Salil.DataAccess.Repository.IRepository;
using Salil.Utility;
using System.Security.Claims;

namespace SalilWeb.ViewComponents
{
    public class ShoppingCartViewComponent:ViewComponent
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public ShoppingCartViewComponent(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // count for shopping cart
            int count = 0;

            if (claim!=null)
            {
                //User Logged in
                if (HttpContext.Session.GetInt32(SD.sessionCart)!=null)
                {
                    // session is not null then get from session

                    return View(HttpContext.Session.GetInt32(SD.sessionCart));

                }

                else
                {
                    //else get it from DB and update the view with count

                     count = _unitOfWorks.shoppingCartRepository.GetAll(
                        u=>u.ApplicationUserId==claim.Value).ToList().Count;

                    HttpContext.Session.SetInt32(SD.sessionCart, count);

                    return View(count);
                    
                }

            }
             
            else 
            {
                HttpContext.Session.Clear();
                //User not logged in then return to view with count
                return View(count);
            }
        }

    }
}
