using DTOs;
using ITI.Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace ITI.Ecommerce.Presenation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
       

        public OrderController(IOrderService orderService , ICustomerService customerService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _paymentService = paymentService;
           
        }

       

        public async Task<IActionResult> GetAllOrders()
        {
            var Orders = await _orderService.GetAll();
            return View(Orders);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderyByID()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetOrderyByID(int ID)
        {
            var Order = await _orderService.GetById(ID);
            ViewBag.Done = true;
            return View(Order);
        }


        public IActionResult Delete(int id)
        {
            _orderService.Delete(id);
            return RedirectToAction("GetAllOrders");
        }
      

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveredOrder()
        {

            var Orders = await _orderService.GetAllDelivered();
            return View(Orders);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPendingOrder()
        {

            var Orders = await _orderService.GetAllPending();
            return View(Orders);
        }

        public ActionResult Delivered(int id)
        {

            _orderService.ChangeToDelivered(id);
            return RedirectToAction("GetAllDeliveredOrder");
        }


       
        public ActionResult Index()
        {
            return View();
        }
    }
}
