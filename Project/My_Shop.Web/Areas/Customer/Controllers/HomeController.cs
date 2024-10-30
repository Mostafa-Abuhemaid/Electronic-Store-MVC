using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using My_Shop.Entities.ViewModels;
using System.Security.Claims;
using X.PagedList;

namespace My_Shop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
       
        private readonly IUniteOfWork _uniteOfWork;
        public HomeController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 8;


            var products = _uniteOfWork.product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }

        public IActionResult Details(int ProductId)
        {
            ShopingCard obj = new ShopingCard() {
                ProductId = ProductId,
                Product = _uniteOfWork.product.GetFristOrDefault(x => x.Id == ProductId, IncliudWord: "Category"),
                Count = 1

            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShopingCard shopingCard)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			shopingCard.ApplicationUserId = claim.Value;

			ShopingCard Cartobj = _uniteOfWork.shopingcard.GetFristOrDefault(
				u => u.ApplicationUserId == claim.Value && u.ProductId == shopingCard.ProductId);
			if (Cartobj == null)
            {
              _uniteOfWork.shopingcard.add(shopingCard);
            }
            else
            {
                _uniteOfWork.shopingcard.Increase(Cartobj, shopingCard.Count);
            }
            
            _uniteOfWork.Compelet();
            return RedirectToAction("Index");

        }

    }
}
