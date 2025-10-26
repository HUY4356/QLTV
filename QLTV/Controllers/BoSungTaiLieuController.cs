using System.Linq;
using System.Web.Mvc;
using QLTV.Models;

namespace QLTV.Controllers
{
    public class BoSungTaiLieuController : Controller
    {
        private QLTVContext db = new QLTVContext();

        public ActionResult Index()
        {
            return View(db.BoSungTaiLieus.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BoSungTaiLieu model)
        {
            if (ModelState.IsValid)
            {
                db.BoSungTaiLieus.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.BoSungTaiLieus.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(BoSungTaiLieu model)
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
            var item = db.BoSungTaiLieus.Find(id);
            db.BoSungTaiLieus.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
