
using BookShelf.DataAccess;
using BookShelf.DataAccess.Repository.IRepository;
using BookShelf.Models;
using BookShelf.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShelf.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class  CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        
        public CompanyController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
            
            
        }
        public IActionResult Index()
        {
            
            return View();
        }
    
        //Get Create and Update
        public IActionResult Upsert(int? id)
        {
           
            Company company = new();
            if(id == null || id == 0)
            {
                //Create Product
                return View(company);
            }
            else
            {
                Company company1= _unitofwork.Company.GetFirstOrDefault(u => u.Id == id);

                return View(company1);
                //Update Product

            }
            
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company, IFormFile? file )
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitofwork.Company.Add(company);
                    TempData["success"] = "Company Created Successfully";
                }
                else
                {
                    _unitofwork.Company.Update(company);
                    TempData["success"] = "Company Updated Successfully";
                }
              
                _unitofwork.Save();
                
                return RedirectToAction("Index");
            }
            return View(company);
        }    
        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = _unitofwork.Company.GetAll();
            return Json(new{ data=CompanyList});
        }
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
            var obj = _unitofwork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            _unitofwork.Company.Remove(obj);
            _unitofwork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
            return RedirectToAction("Index");


        }
        #endregion


    }
}
