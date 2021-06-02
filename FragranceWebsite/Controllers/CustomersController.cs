using FragranceWebsite.Repository.IRepository;
using FragranceWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomersController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public IActionResult Index()
        {
            if (this.HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Home");

            return View(new CustomerVM() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            return Json(new { data = await _customerRepo.GetAllAsync(StaticInfo.CustomerAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            if (this.HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Home");

            CustomerVM customerVM = new CustomerVM();

            if (id == null)
            {
                return View(customerVM);
            }

            customerVM = await _customerRepo.GetAsync(StaticInfo.CustomerAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if(customerVM == null)
            {
                return NotFound();
            }

            return View(customerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                if(customerVM.CustomerId == 0)
                {
                    await _customerRepo.CreateAsync(StaticInfo.CustomerAPIPath, customerVM, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _customerRepo.UpdateAsync(StaticInfo.CustomerAPIPath + customerVM.CustomerId, customerVM, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction("Index");
            }

            return View(customerVM);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _customerRepo.DeleteAsync(StaticInfo.CustomerAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
                return Json(new { status = true, message = "Delete Successful" });

            return Json(new { status = false, message = "Delete Not Successful" });
        }
    }
}
