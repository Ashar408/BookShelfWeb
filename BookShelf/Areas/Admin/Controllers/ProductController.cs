
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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
            
        }
        public IActionResult Index()
        {
            
            return View();
        }
    
        //Get Create and Update
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _unitofwork.Category.GetAll().Select(
                u=> new SelectListItem
                {
                    Text=u.Name,
                    Value=u.Id.ToString()
                }
                );
            IEnumerable<SelectListItem> CopyTypeList = _unitofwork.CopyType.GetAll().Select(
                u => new SelectListItem
                 {
                    Text = u.Name,
                    Value = u.Id.ToString()
                 }
                 );

            Product product = new();
            if(id == null || id == 0)
            {
                //Create Product
                ViewBag.CategoryList=CategoryList;
                ViewBag.CopyTypeList=CopyTypeList;
                return View(product);
            }
            else
            {
                Product product1= _unitofwork.Product.GetFirstOrDefault(u => u.Id == id);
                ViewBag.CategoryList = CategoryList;
                ViewBag.CopyTypeList = CopyTypeList;
                return View(product1);
                //Update Product

            }
            
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product, IFormFile? file )
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(wwwRootPath,@"images\products");
                    var extension=Path.GetExtension(file.FileName);

                    if (product.ImageUrl != null)
                    {
                        var OldImage = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(OldImage))
                        {
                            System.IO.File.Delete(OldImage);
                        }
                    }


                    using(var fileStream = new FileStream(Path.Combine(upload,fileName+extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.ImageUrl = @"images\products\" + fileName + extension;
                }
                if (product.Id == 0)
                {
                    _unitofwork.Product.Add(product);
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitofwork.Product.Update(product);
                    TempData["success"] = "Product Updated Successfully";
                }
              
                _unitofwork.Save();
                
                return RedirectToAction("Index");
            }
            return View(product);
        }    
        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitofwork.Product.GetAll(includeProperties: "Category,CopyType");
            return Json(new{ data=productList});
        }
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
            var obj = _unitofwork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var OldImage = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(OldImage))
            {
                System.IO.File.Delete(OldImage);
            }
            _unitofwork.Product.Remove(obj);
            _unitofwork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
            return RedirectToAction("Index");


        }
        #endregion


    }
}
