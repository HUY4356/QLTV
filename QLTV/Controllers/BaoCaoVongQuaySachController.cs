using System.Linq;
using System.Web.Mvc;
using QLTV.Models;

namespace QLTV.Controllers
{
    public class BaoCaoVongQuaySachController : Controller
    {
        private QLTVContext db = new QLTVContext();

        public ActionResult Index()
        {
            return View(db.BaoCaoVongQuaySaches.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BaoCaoVongQuaySach model)
        {
            if (ModelState.IsValid)
            {
                db.BaoCaoVongQuaySaches.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.BaoCaoVongQuaySaches.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(BaoCaoVongQuaySach model)
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
            var item = db.BaoCaoVongQuaySaches.Find(id);
            db.BaoCaoVongQuaySaches.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
