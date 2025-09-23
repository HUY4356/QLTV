using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QL_ThuVien.Models;

namespace QL_ThuVien.Controllers
{
    public class QLThuVienController : Controller
    {
        private readonly ILogger<QLThuVienController> _logger;

        public QLThuVienController(ILogger<QLThuVienController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
