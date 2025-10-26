using System.Web.Mvc;

namespace QLTV.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Giới thiệu hệ thống quản lý thư viện.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Liên hệ quản trị hệ thống.";
            return View();
        }
    }
}
