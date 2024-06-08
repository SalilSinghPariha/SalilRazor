using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using Salil.Model.ViewModel;
using Salil.Utility;

namespace SalilWeb.Pages.Admin.Order
{
    [Authorize(Roles =$"{SD.managerRole},{SD.kitchenRole}")]
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public List<OrderDetailsVM> OrderDetailsVM { get; set; }
        public ManageOrderModel(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            
        }
        public void OnGet()
        {
            OrderDetailsVM = new();

            //get the order based on status
            List<OrderHeader> orderHeaders= _unitOfWorks.orderHeaderRepository.GetAll(
                u=>u.OrderStatus==SD.statusSubmitted || u.OrderStatus==SD.statusInProcess
                ).ToList();

            //then loop through each reacord and get the details

            foreach (var item in orderHeaders)
            {

                OrderDetailsVM individual = new OrderDetailsVM()
                {
                    OrderHeader=item,
                    OrderDetails=_unitOfWorks.orderDetailsRepository.GetAll(u=>u.OrderId==item.Id).ToList()

                };

               OrderDetailsVM.Add(individual);
            }
        }

        public IActionResult OnPostOrderInProcess(int orderId)
        {
            _unitOfWorks.orderHeaderRepository.UpdateStatus(orderId, SD.statusInProcess);
            _unitOfWorks.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderReady(int orderId)
        {
            _unitOfWorks.orderHeaderRepository.UpdateStatus(orderId, SD.statusReady);
            _unitOfWorks.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            _unitOfWorks.orderHeaderRepository.UpdateStatus(orderId, SD.statusCancelled);
            _unitOfWorks.Save();
            return RedirectToPage("ManageOrder");
        }
    }
}