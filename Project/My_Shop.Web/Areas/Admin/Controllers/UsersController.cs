using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Shop.DataAccess;
using My_Shop.Utilities;
using System.Security.Claims;

namespace My_Shop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;

            return View(_context.applicationUsers.Where(x => x.Id != userid).ToList());
        }
        public IActionResult LockUnlock(string? id)
        {
            var user = _context.applicationUsers.FirstOrDefault(U => U.Id == id);
            if (user == null)
                return NotFound();

            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddHours(4);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }
    }
}