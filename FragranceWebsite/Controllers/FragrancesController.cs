using FragranceWebsite.Repository.IRepository;
using FragranceWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.Controllers
{
    [Authorize]
    public class FragrancesController : Controller
    {
        private readonly IFragranceRepository _fragranceRepo;

        public FragrancesController(IFragranceRepository fragranceRepo)
        {
            _fragranceRepo = fragranceRepo;
        }

        public IActionResult Index()
        {
            if (this.HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Home");

            return View(new FragranceVM() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFragrances()
        {
            return Json(new { data = await _fragranceRepo.GetAllAsync(StaticInfo.FragranceAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            if (this.HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Home");

            FragranceVM fragranceVM = new FragranceVM();

            if (id == null)
            {
                return View(fragranceVM);
            }
            
            fragranceVM = await _fragranceRepo.GetAsync(StaticInfo.FragranceAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (fragranceVM == null)
            {
                return NotFound();
            }
              
            return View(fragranceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(FragranceVM fragranceVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    byte[] p1 = null;
                    using(var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        } 
                    }
                    fragranceVM.Picture = p1;
                }
                else
                {
                    var objFromDb = await _fragranceRepo.GetAsync(StaticInfo.FragranceAPIPath, fragranceVM.FragranceId, HttpContext.Session.GetString("JWToken"));
                    fragranceVM.Picture = objFromDb.Picture;
                }
                if(fragranceVM.FragranceId == 0)
                {
                    await _fragranceRepo.CreateAsync(StaticInfo.FragranceAPIPath, fragranceVM, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _fragranceRepo.UpdateAsync(StaticInfo.FragranceAPIPath + fragranceVM.FragranceId, fragranceVM, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction("Index");
            }
            
                return View(fragranceVM);
            
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _fragranceRepo.DeleteAsync(StaticInfo.FragranceAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
                return Json(new { status = true, message = "Delete Successful" });

            return Json(new { status = false, message = "Delete Not Successful" });
        }
    }
}
