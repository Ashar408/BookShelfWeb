using BookShelf.DataAccess.Repository.IRepository;
using BookShelf.Models;
using BookShelf.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookShelf.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitofwork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties:"Category,CopyType");
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cartobj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitofwork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CopyType"),
            };
            return View(cartobj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;
            ShoppingCart cartFromDb=_unitofwork.ShoppingCart.GetFirstOrDefault(u=>u.ApplicationUserId == claim.Value && u.ProductId==shoppingCart.ProductId);
            if (cartFromDb == null)
            {
                _unitofwork.ShoppingCart.Add(shoppingCart);
                _unitofwork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
            }
            else
            {
                _unitofwork.ShoppingCart.IncrementCount(cartFromDb,shoppingCart.Count);
                _unitofwork.Save();
            }
            
           
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}