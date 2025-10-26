using System.Linq;
using System.Web.Mvc;
using QLTV.Models;

namespace QLTV.Controllers
{
    public class NhomSachController : Controller
    {
        private QLTVContext db = new QLTVContext();

        public ActionResult Index()
        {
            return View(db.NhomSachs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NhomSach model)
        {
            if (ModelState.IsValid)
            {
                db.NhomSachs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.NhomSachs.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(NhomSach model)
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
            var item = db.NhomSachs.Find(id);
            db.NhomSachs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
