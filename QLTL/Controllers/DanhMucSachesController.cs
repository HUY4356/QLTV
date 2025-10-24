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
    public class DanhMucSachesController : Controller
    {
        private QLTVContext db = new QLTVContext();

        // GET: DanhMucSaches
        public ActionResult Index()
        {
            return View(db.DanhMucSaches.ToList());
        }

        // GET: DanhMucSaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSach danhMucSach = db.DanhMucSaches.Find(id);
            if (danhMucSach == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSach);
        }

        // GET: DanhMucSaches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DanhMucSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NhomSach_id,MaSach,TenSach,TacGia,DonGia")] DanhMucSach danhMucSach)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucSaches.Add(danhMucSach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danhMucSach);
        }

        // GET: DanhMucSaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSach danhMucSach = db.DanhMucSaches.Find(id);
            if (danhMucSach == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSach);
        }

        // POST: DanhMucSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NhomSach_id,MaSach,TenSach,TacGia,DonGia")] DanhMucSach danhMucSach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMucSach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMucSach);
        }

        // GET: DanhMucSaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSach danhMucSach = db.DanhMucSaches.Find(id);
            if (danhMucSach == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSach);
        }

        // POST: DanhMucSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMucSach danhMucSach = db.DanhMucSaches.Find(id);
            db.DanhMucSaches.Remove(danhMucSach);
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
