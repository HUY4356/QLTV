using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QL_ThuVien.Models;
using System.Linq;

namespace QL_ThuVien.Controllers
{
    public class AuthController : Controller
    {
        private readonly ThuVienDbContext _context;
        public AuthController(ThuVienDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && user.Password == password) // 🔒 có thể thay bằng BCrypt check
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);
                HttpContext.Session.SetString("Fullname", user.Fullname);

                if (user.RoleId == 1) return RedirectToAction("Index", "Home"); // Admin
                return RedirectToAction("Index", "Home"); // User
            }
            ViewBag.Error = "Email hoặc mật khẩu không đúng";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
