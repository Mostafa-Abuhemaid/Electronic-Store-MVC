using Abp.Domain.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Shop.Entities.Repository;
using My_Shop.Utilities;

namespace My_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {

        private readonly IUniteOfWork _uniteOfWork;

        public DashboardController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Orders =_uniteOfWork.OrderHeader.GetAll().Count();
            ViewBag.ApprovedOrders = _uniteOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.Approve).Count();
            ViewBag.Users = _uniteOfWork.ApplicationUser.GetAll().Count();
            ViewBag.Products = _uniteOfWork.product.GetAll().Count();
            return View();
        }
    }
}
