using Abp.Domain.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Shop.DataAccess.Implementation;
using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using My_Shop.Entities.ViewModels;
using My_Shop.Utilities;
using Stripe.Checkout;
using System.Security.Claims;

namespace My_Shop.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUniteOfWork _uniteOfWork;
		public ShopingCardVM ShoppingCartVM { get; set; }
		public CartController(IUniteOfWork uniteOfWork)
		{
			_uniteOfWork = uniteOfWork;
		}

		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShopingCardVM()
			{
				CartsList = _uniteOfWork.shopingcard.GetAll(u => u.ApplicationUserId == claim.Value, IncliudWord: "Product")


			};
			foreach (var item in ShoppingCartVM.CartsList)
			{
				//(item.Count * item.Product.Price);
				ShoppingCartVM.TotalCard += (item.Count * item.Product.Price);
			}

			return View(ShoppingCartVM);
		}


        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShopingCardVM()
            {
                CartsList = _uniteOfWork.shopingcard.GetAll(u => u.ApplicationUserId == claim.Value, IncliudWord: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _uniteOfWork.ApplicationUser.GetFristOrDefault(x => x.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);// (item.Count * item.Product.Price);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult POSTSummary(ShopingCardVM ShoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.CartsList = _uniteOfWork.shopingcard.GetAll(u => u.ApplicationUserId == claim.Value, IncliudWord: "Product");


            ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            _uniteOfWork.OrderHeader.add(ShoppingCartVM.OrderHeader);
            _uniteOfWork.Compelet();

            foreach (var item in ShoppingCartVM.CartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                _uniteOfWork.OrderDetail.add(orderDetail);
                _uniteOfWork.Compelet();
            }

            var domain = "https://localhost:44319/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"Customer/Cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"Customer/Cart/index",
            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;

            _uniteOfWork.Compelet();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.CartsList);
            //         _unitOfWork.Complete();
            //         return RedirectToAction("Index","Home");

        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _uniteOfWork.OrderHeader.GetFristOrDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _uniteOfWork.OrderHeader.UpdateStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _uniteOfWork.Compelet();
            }
            List<ShopingCard> shoppingcarts = _uniteOfWork.shopingcard.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
  
            _uniteOfWork.shopingcard.RemoveInRange(shoppingcarts);
            _uniteOfWork.Compelet();
            return View(id);

        }
            public IActionResult Plus(int cartid)
		{
			var shoppingcart = _uniteOfWork.shopingcard.GetFristOrDefault(x => x.Id == cartid);
			_uniteOfWork.shopingcard.Increase(shoppingcart, 1);
			_uniteOfWork.Compelet();
			return RedirectToAction("Index");
		}
		public IActionResult Minus(int cartid)
		{
			var shoppingcart = _uniteOfWork.shopingcard.GetFristOrDefault(x => x.Id == cartid);

			if (shoppingcart.Count <= 1)
			{
				_uniteOfWork.shopingcard.Remove(shoppingcart);
				_uniteOfWork.Compelet();
				return RedirectToAction("Index", "Home");
			}
			else
			{
				_uniteOfWork.shopingcard.Decrease(shoppingcart, 1);

			}
			_uniteOfWork.Compelet();
			return RedirectToAction("Index");
		}
		public IActionResult Remove(int cartid)
		{
			var shoppingcart = _uniteOfWork.shopingcard.GetFristOrDefault(x => x.Id == cartid);
			_uniteOfWork.shopingcard.Remove(shoppingcart);
			_uniteOfWork.Compelet();

			return RedirectToAction("Index");
		}
		
	}
}