using System.Linq;
using System.Web.Mvc;
using QLTV.Models;

namespace QLTV.Controllers
{
    public class DanhMucSachController : Controller
    {
        private QLTVContext db = new QLTVContext();

        public ActionResult Index()
        {
            return View(db.DanhMucSachs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DanhMucSach model)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucSachs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.DanhMucSachs.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(DanhMucSach model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var item = db.DanhMucSachs.Find(id);
            db.DanhMucSachs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
