using FragranceWebsite.Repository.IRepository;
using FragranceWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IFragranceRepository _fragranceRepo;
        private readonly ICustomerRepository _customerRepo;

        public SalesController(ISaleRepository saleRepo, IFragranceRepository fragranceRepo, ICustomerRepository customerRepo)
        {
            _saleRepo = saleRepo;
            _fragranceRepo = fragranceRepo;
            _customerRepo = customerRepo;
        }

        public IActionResult Index()
        {
            if (this.HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Home");

            return View(new Sale() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            return Json(new { data = await _saleRepo.GetAllAsync(StaticInfo.SaleAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            if (this.HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Home");

            IEnumerable<FragranceVM> frList = (IEnumerable<FragranceVM>)await _fragranceRepo.GetAllAsync(StaticInfo.FragranceAPIPath, HttpContext.Session.GetString("JWToken"));
            IEnumerable<CustomerVM> cmList = (IEnumerable<CustomerVM>)await _customerRepo.GetAllAsync(StaticInfo.CustomerAPIPath, HttpContext.Session.GetString("JWToken"));

            SaleVM saleVM = new SaleVM()
            {
                FragranceList = frList.Select(i => new SelectListItem
                {
                    Text = i.Brand + "/" + i.Name + "/" + i.Price,
                    Value = i.FragranceId.ToString()
                }),
                CustomerList = cmList.Select(i => new SelectListItem
                {
                    Text = i.FirstName + " " + i.LastName,
                    Value = i.CustomerId.ToString()
                }),
                Sale = new Sale()
            };

            if(id == null)
            {
                return View(saleVM);
            }

            saleVM.Sale = await _saleRepo.GetAsync(StaticInfo.SaleAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if(saleVM.Sale == null)
            {
                return NotFound();
            }

            return View(saleVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SaleVM saleVM)
        {
            if (ModelState.IsValid)
            {
                if (saleVM.Sale.SaleId == 0)
                {
                    await _saleRepo.CreateAsync(StaticInfo.SaleAPIPath, saleVM.Sale, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _saleRepo.UpdateAsync(StaticInfo.SaleAPIPath + saleVM.Sale.SaleId, saleVM.Sale, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<FragranceVM> frList = (IEnumerable<FragranceVM>)await _fragranceRepo.GetAllAsync(StaticInfo.FragranceAPIPath, HttpContext.Session.GetString("JWToken"));
                IEnumerable<CustomerVM> cmList = (IEnumerable<CustomerVM>)await _customerRepo.GetAllAsync(StaticInfo.CustomerAPIPath, HttpContext.Session.GetString("JWToken"));

                SaleVM objVM = new SaleVM()
                {
                    FragranceList = frList.Select(i => new SelectListItem
                    {
                        Text = i.Brand + "/" + i.Name + "/" + i.Price,
                        Value = i.FragranceId.ToString()
                    }),
                    CustomerList = cmList.Select(i => new SelectListItem
                    {
                        Text = i.FirstName + " " + i.LastName,
                        Value = i.CustomerId.ToString()
                    }),
                    Sale = saleVM.Sale
                };
                return View(objVM);
            } 
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _saleRepo.DeleteAsync(StaticInfo.SaleAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
                return Json(new { status = true, message = "Delete Successful" });

            return Json(new { status = false, message = "Delete Not Successful" });
        }
    }
}
