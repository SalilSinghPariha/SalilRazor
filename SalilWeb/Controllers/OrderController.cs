using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salil.DataAccess.Repository.IRepository;
using Salil.Utility;

namespace SalilWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IUnitOfWorks _unitOfWorks;
        
        public OrderController(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(string? status=null)
        {
            // Return all Order list item
            var orderList = _unitOfWorks.orderHeaderRepository.GetAll(includeProperites: "applicationUser");

            //if status is passed then fetched based on status

            if (status == "cancelled")
            {
                orderList = _unitOfWorks.orderHeaderRepository.GetAll(u => u.OrderStatus == SD.statusCancelled
                || u.OrderStatus == SD.statusRejected);
            }
            else
            {
                if (status == "completed")
                {
                    orderList = _unitOfWorks.orderHeaderRepository.GetAll(u => u.OrderStatus == SD.statusCompleted);
                }
                else
                {
                    if (status == "ready")
                    {
                        orderList = _unitOfWorks.orderHeaderRepository.GetAll(u => u.OrderStatus == SD.statusReady);
                    }

                    else
                    {
                        orderList = _unitOfWorks.orderHeaderRepository.GetAll(u=>u.OrderStatus==SD.statusInProcess
                        ||u.OrderStatus==SD.statusSubmitted);
                    }

                }
                    
            }
            // then return items as json

            return Json(new { data = orderList });
        }
       
    }
}
