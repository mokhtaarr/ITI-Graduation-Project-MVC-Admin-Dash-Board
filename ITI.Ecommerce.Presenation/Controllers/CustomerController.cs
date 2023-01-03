using Microsoft.AspNetCore.Mvc;
using DTOs;
using ITI.Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ITI.Ecommerce.Presenation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
       public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
           
            var User = await _customerService.GetAll();
          
            return View(User);
        }

       [HttpGet]



        public async Task<IActionResult> UpdateUser(string ID)
        {
            var User = await _customerService.GetById(ID);

            return View(User);
        }
        [HttpPost]
        public IActionResult UpdateUser(CustomerDto customerDto)
        {
              _customerService.Update(customerDto);

            return RedirectToAction("GetAllUser","Customer");
        }
       
      
        public IActionResult Delete(string ID)
        {
            _customerService.Delete(ID);

            return RedirectToAction("GetAllUser", "Customer");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserDeleted()
        {
            
            var User = await _customerService.GetAllDeleted();
           
            return View(User);
        }

        public async Task<IActionResult> Restore(string ID)
        {
            _customerService.Restore(ID);

            return RedirectToAction("GetAllUserDeleted", "Customer");

        }

    }
}
