using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using Salil.Utility;
using Stripe.Checkout;
using System.Security.Claims;

namespace SalilWeb.Pages.Customer.Cart
{
    [Authorize]
    [BindProperties]
    public class SummaryModel : PageModel
    {

        private readonly IUnitOfWorks _unitOfWorks;

        public SummaryModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            OrderHeader = new OrderHeader();

        }
        public IEnumerable<ShoppingCart> ShoppingCartLists { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public void OnGet()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ShoppingCartLists = _unitOfWorks.shoppingCartRepository.GetAll(
                    filter: u => u.ApplicationUserId == claim.Value,
                    includeProperites: "menuItem,menuItem.category,menuItem.foodType"
                    );

                foreach (var cartitem in ShoppingCartLists)
                {
                    OrderHeader.OrderTotal += (cartitem.menuItem.Price * cartitem.Count);
                }
                ApplicationUser applicationUser = _unitOfWorks.applicationUserRepository.GetFirstOrDefault(
                    u=>u.Id==claim.Value
                    );
                OrderHeader.PickUpName = applicationUser.firstName+""+applicationUser.lastName;
                OrderHeader.PhoneNumber = applicationUser.PhoneNumber;

            }
        }

        public IActionResult OnPost() 
        {
			var ClaimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ShoppingCartLists = _unitOfWorks.shoppingCartRepository.GetAll(
                    filter: u => u.ApplicationUserId == claim.Value,
                    includeProperites: "menuItem,menuItem.category,menuItem.foodType"
                    );

                foreach (var cartitem in ShoppingCartLists)
                {
                    OrderHeader.OrderTotal += (cartitem.menuItem.Price * cartitem.Count);
                }

           
           // OrderHeader.PhoneNumber = OrderHeader.PhoneNumber;
            OrderHeader.OrderStatus = SD.statusPending;
            OrderHeader.UserId = claim.Value;
            OrderHeader.OrderDate = DateTime.Now;
            OrderHeader.PickupTime = Convert.ToDateTime(OrderHeader.PickUpDate.ToShortDateString() + " " +
                         OrderHeader.PickupTime.ToShortTimeString());

                _unitOfWorks.orderHeaderRepository.Add( OrderHeader );
            _unitOfWorks.Save();

            foreach (var details in ShoppingCartLists)
            {
                OrderDetails orderDetails = new()
                {
                    OrderId = OrderHeader.Id,
                    MenuItemId= details.MenuItemId,
                    Name=details.menuItem.Name,
                    Price=details.menuItem.Price,
                    count=details.Count
				};

                _unitOfWorks.orderDetailsRepository.Add( orderDetails );
               
            }
            //Count is required for payment
            //int quantity = ShoppingCartLists.ToList().Count;

           // _unitOfWorks.shoppingCartRepository.RemoveRange(ShoppingCartLists);
            _unitOfWorks.Save();

			var domain = "https://localhost:7059/";
				var options = new SessionCreateOptions
				{
					LineItems = new List<SessionLineItemOptions>()
				   ,
					PaymentMethodTypes = new List<string>
				{
				  "card",
				},
					Mode = "payment",
					SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={OrderHeader.Id}",
					CancelUrl = domain + "customer/cart/index",
				};
				//add line Item
				foreach (var item in ShoppingCartLists)
                {
                   var seesionLineItem= new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.menuItem.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.menuItem.Name,
                                //Description= "Total Distinct Item-" +quantity
                            },
                        },

                        Quantity = item.Count
                    };
                    options.LineItems.Add(seesionLineItem);

                }
			var service = new SessionService();
			Session session = service.Create(options);

			Response.Headers.Add("Location", session.Url);
               OrderHeader.SessionId = session.Id;
               OrderHeader.PaymentIntentId = session.PaymentIntentId;
				_unitOfWorks.Save();
				return new StatusCodeResult(303);
			}

            
            return Page();

		}
    }
}
