
using BookShelf.DataAccess;
using BookShelf.DataAccess.Repository.IRepository;
using BookShelf.Models;
using BookShelf.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShelf.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CopyTypeController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CopyTypeController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<CopyType> objcopytype = _unitofwork.CopyType.GetAll();
            return View(objcopytype);
        }
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CopyType copyType)
        {

            if (ModelState.IsValid)
            {
                _unitofwork.CopyType.Add(copyType);
                _unitofwork.Save();
                TempData["success"] = "Copy Type Created Successfully";
                return RedirectToAction("Index");
            }
            return View(copyType);
        }     
        
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var copyTypefromDb = _unitofwork.CopyType.GetFirstOrDefault(u => u.Id == id);
            if (copyTypefromDb == null)
            {
                return NotFound();
            }
            return View(copyTypefromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CopyType copyType)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.CopyType.Update(copyType);
                _unitofwork.Save();
                TempData["success"] = "Copy Type Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(copyType);
        }    
        
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var copyTypefromDb = _unitofwork.CopyType.GetFirstOrDefault(u => u.Id == id);
            if (copyTypefromDb == null)
            {
                return NotFound();
            }
            return View(copyTypefromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitofwork.CopyType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.CopyType.Remove(obj);
                _unitofwork.Save();
            TempData["success"] = "Copy Type Deleted Successfully";
            return RedirectToAction("Index");
            
          
        }


    }
}
