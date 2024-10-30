using Microsoft.AspNetCore.Mvc;
using My_Shop.DataAccess    ;
using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;

namespace My_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController1 : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        public CategoryController1(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;

        }
        public IActionResult Index()
        {
            var category = _uniteOfWork.category.GetAll();
            return View(category);
        }
        [HttpGet]
        public IActionResult Creat()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Creat(Category category)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.category.add(category);
                _uniteOfWork.Compelet();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edite(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var categorybyid = _uniteOfWork.category.GetFristOrDefault(x => x.Id == id);
            return View(categorybyid);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edite(Category category)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.category.Update(category);
                
                _uniteOfWork.Compelet();
                TempData["massage"] = "Data has Deleted";
                return RedirectToAction("Index");
             
            }
            return View();
        }
        //
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var categorybyid = _uniteOfWork.category.GetFristOrDefault(x => x.Id == id);
            return View(categorybyid);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var categorybyid = _uniteOfWork.category.GetFristOrDefault(x => x.Id == id);
            if (ModelState.IsValid)
            {
                _uniteOfWork.category.Remove(categorybyid);
                _uniteOfWork.Compelet();
                TempData["massage"] = "Data has Deleted";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
