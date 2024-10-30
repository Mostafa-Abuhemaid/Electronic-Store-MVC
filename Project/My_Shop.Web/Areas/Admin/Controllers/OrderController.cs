using Microsoft.AspNetCore.Mvc;
using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using My_Shop.Entities.ViewModels;

namespace My_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;

        public OrderController(IUniteOfWork uniteOfWork)
        {
          _uniteOfWork = uniteOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _uniteOfWork.OrderHeader.GetAll(IncliudWord: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }
        public IActionResult Details(int orderid)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _uniteOfWork.OrderHeader.GetFristOrDefault(u => u.Id == orderid, IncliudWord: "ApplicationUser"),
                orderDetails = _uniteOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == orderid, IncliudWord: "Product")
            };

            return View(orderVM);
        }

    }
}
