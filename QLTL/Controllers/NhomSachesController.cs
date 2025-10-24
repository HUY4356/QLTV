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
    public class NhomSachesController : Controller
    {
        private QLTVContext db = new QLTVContext();

        // GET: NhomSaches
        public ActionResult Index()
        {
            return View(db.NhomSaches.ToList());
        }

        // GET: NhomSaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomSach nhomSach = db.NhomSaches.Find(id);
            if (nhomSach == null)
            {
                return HttpNotFound();
            }
            return View(nhomSach);
        }

        // GET: NhomSaches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhomSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaNhom,TenNhom")] NhomSach nhomSach)
        {
            if (ModelState.IsValid)
            {
                db.NhomSaches.Add(nhomSach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhomSach);
        }

        // GET: NhomSaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomSach nhomSach = db.NhomSaches.Find(id);
            if (nhomSach == null)
            {
                return HttpNotFound();
            }
            return View(nhomSach);
        }

        // POST: NhomSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaNhom,TenNhom")] NhomSach nhomSach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhomSach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhomSach);
        }

        // GET: NhomSaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomSach nhomSach = db.NhomSaches.Find(id);
            if (nhomSach == null)
            {
                return HttpNotFound();
            }
            return View(nhomSach);
        }

        // POST: NhomSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhomSach nhomSach = db.NhomSaches.Find(id);
            db.NhomSaches.Remove(nhomSach);
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
