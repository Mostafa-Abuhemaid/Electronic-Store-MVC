using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;

using My_Shop.DataAccess;
using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using My_Shop.Entities.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace My_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUniteOfWork uniteOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _uniteOfWork = uniteOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            var categories = _uniteOfWork.product.GetAll(IncliudWord: "Category");

            return Json(new { data = categories });
        }
        [HttpGet]
      
        public IActionResult Creat()
        {
            ProductVM productVM = new ProductVM()
            {
                product = new Product(),
                CategoryList = _uniteOfWork.category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })


            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Creat(ProductVM productVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Product");
                    var ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.product.Img = @"Images\Product\" + filename + ext;
                }

                _uniteOfWork.product.add(productVM.product);
                _uniteOfWork.Compelet();
                return RedirectToAction("Index");
            }
            return View(productVM.product);
        }
        [HttpGet]
        public IActionResult Edite(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            ProductVM productVM = new ProductVM()
            {
                product = _uniteOfWork.product.GetFristOrDefault(x => x.Id==id),
                CategoryList = _uniteOfWork.category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })


            };
            return View(productVM);
      
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edite(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootpath, @"Images\Product\");
                    var ext = Path.GetExtension(file.FileName);
                    if (productVM.product.Img != null)
                    {
                        var old = Path.Combine(rootpath, productVM.product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(old))
                        {
                            System.IO.File.Delete(old);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.product.Img = @"\Images\Product\" + filename + ext;
                }
                _uniteOfWork.product.Update(productVM.product);
                _uniteOfWork.Compelet();
                return RedirectToAction("Index");
            }
            return View(productVM.product);

        }
        //
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _uniteOfWork.product.GetFristOrDefault(X => X.Id == id);
            if (product == null)
                return Json(new { success = false, message = "Error while deleting the product" });
            _uniteOfWork.product.Remove(product);
            var old = Path.Combine(_webHostEnvironment.WebRootPath, product.Img.TrimStart('\\'));
            if (System.IO.File.Exists(old))
            {
                System.IO.File.Delete(old);
            }
            //  DocumentSettings.DeleteFile(product.ImageName, "Products");
            _uniteOfWork.Compelet();
            return Json(new { success = true, message = "product has been deleted succesfully" });
        }

    }
}
