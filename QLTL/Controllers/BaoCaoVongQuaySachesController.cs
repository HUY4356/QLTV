using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLTV_Backend.Models;

namespace QLTL.Controllers
{
    public class BaoCaoVongQuaySachesController : Controller
    {
        private QLTVContext db = new QLTVContext();

        // GET: BaoCaoVongQuaySaches
        public ActionResult Index()
        {
            return View(db.BaoCaoVongQuaySaches.ToList());
        }

        // GET: BaoCaoVongQuaySaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoVongQuaySach baoCaoVongQuaySach = db.BaoCaoVongQuaySaches.Find(id);
            if (baoCaoVongQuaySach == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoVongQuaySach);
        }

        // GET: BaoCaoVongQuaySaches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BaoCaoVongQuaySaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DanhMucSach_id,ThongKeLuotMuon")] BaoCaoVongQuaySach baoCaoVongQuaySach)
        {
            if (ModelState.IsValid)
            {
                db.BaoCaoVongQuaySaches.Add(baoCaoVongQuaySach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(baoCaoVongQuaySach);
        }

        // GET: BaoCaoVongQuaySaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoVongQuaySach baoCaoVongQuaySach = db.BaoCaoVongQuaySaches.Find(id);
            if (baoCaoVongQuaySach == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoVongQuaySach);
        }

        // POST: BaoCaoVongQuaySaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DanhMucSach_id,ThongKeLuotMuon")] BaoCaoVongQuaySach baoCaoVongQuaySach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baoCaoVongQuaySach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(baoCaoVongQuaySach);
        }

        // GET: BaoCaoVongQuaySaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoVongQuaySach baoCaoVongQuaySach = db.BaoCaoVongQuaySaches.Find(id);
            if (baoCaoVongQuaySach == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoVongQuaySach);
        }

        // POST: BaoCaoVongQuaySaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaoCaoVongQuaySach baoCaoVongQuaySach = db.BaoCaoVongQuaySaches.Find(id);
            db.BaoCaoVongQuaySaches.Remove(baoCaoVongQuaySach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
