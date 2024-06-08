using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using Salil.Utility;
using Stripe.Checkout;

namespace SalilWeb.Pages.Customer.Cart
{
    public class OrderConfirmationModel : PageModel
    {

		private readonly IUnitOfWorks _unitOfWorks;
		public int OrderId { get; set; }	

		public OrderConfirmationModel(IUnitOfWorks unitOfWorks)
		{
			_unitOfWorks = unitOfWorks;

		}
		public void OnGet(int id)
        {
			OrderHeader orderHeader = _unitOfWorks.orderHeaderRepository.GetFirstOrDefault(
				u=>u.Id==id
				);

			if (orderHeader.SessionId != null)
			{
				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus=="paid")
                {
					orderHeader.OrderStatus = SD.statusSubmitted;
					orderHeader.PaymentIntentId = session.PaymentIntentId;
					_unitOfWorks.Save();
				}				
            }

			List<ShoppingCart> shoppingCarts= _unitOfWorks.shoppingCartRepository.GetAll(
				filter: u=>u.ApplicationUserId==orderHeader.UserId
				).ToList();

			_unitOfWorks.shoppingCartRepository.RemoveRange(shoppingCarts);

			_unitOfWorks.Save();

			OrderId = id;
        }
    }
}
