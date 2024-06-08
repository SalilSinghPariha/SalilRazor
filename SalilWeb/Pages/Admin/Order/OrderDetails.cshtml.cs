using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using Salil.Model.ViewModel;
using Salil.Utility;
using Stripe;

namespace SalilWeb.Pages.Admin.Order
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public OrderDetailsVM orderDetaisVM { get; set; }
        public OrderDetailsModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }
        public void OnGet(int id)
        {
            orderDetaisVM = new()
            {
                OrderHeader = _unitOfWorks.orderHeaderRepository.GetFirstOrDefault(
                    u => u.Id == id,includeProperites:"applicationUser"
                    ),
                OrderDetails = _unitOfWorks.orderDetailsRepository.GetAll(u=>u.OrderId==id).ToList(),
            };

        }

        public IActionResult OnPostOrderComplete(int orderId)
        {
            _unitOfWorks.orderHeaderRepository.UpdateStatus(orderId, SD.statusCompleted);
            _unitOfWorks.Save();

            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            _unitOfWorks.orderHeaderRepository.UpdateStatus(orderId, SD.statusCancelled);
            _unitOfWorks.Save();

            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderRefund(int orderId)
        {
            OrderHeader orderHeader = _unitOfWorks.orderHeaderRepository.GetFirstOrDefault(u => u.Id == orderId);

            var option = new RefundCreateOptions
            {
                Reason=RefundReasons.RequestedByCustomer,
                PaymentIntent= orderHeader.PaymentIntentId
            };

            var service = new RefundService();

            Refund refund= service.Create(option);

            _unitOfWorks.orderHeaderRepository.UpdateStatus(orderId, SD.statusRefunded);
            _unitOfWorks.Save();

            return RedirectToPage("OrderList");
        }

    }
}
