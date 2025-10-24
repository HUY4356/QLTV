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
    public class BoSungTaiLieuxController : Controller
    {
        private QLTVContext db = new QLTVContext();

        // GET: BoSungTaiLieux
        public ActionResult Index()
        {
            return View(db.BoSungTaiLieus.ToList());
        }

        // GET: BoSungTaiLieux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoSungTaiLieu boSungTaiLieu = db.BoSungTaiLieus.Find(id);
            if (boSungTaiLieu == null)
            {
                return HttpNotFound();
            }
            return View(boSungTaiLieu);
        }

        // GET: BoSungTaiLieux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoSungTaiLieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeXuat,XuLy,GiaiPhap,NoiDung")] BoSungTaiLieu boSungTaiLieu)
        {
            if (ModelState.IsValid)
            {
                db.BoSungTaiLieus.Add(boSungTaiLieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(boSungTaiLieu);
        }

        // GET: BoSungTaiLieux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoSungTaiLieu boSungTaiLieu = db.BoSungTaiLieus.Find(id);
            if (boSungTaiLieu == null)
            {
                return HttpNotFound();
            }
            return View(boSungTaiLieu);
        }

        // POST: BoSungTaiLieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeXuat,XuLy,GiaiPhap,NoiDung")] BoSungTaiLieu boSungTaiLieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boSungTaiLieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boSungTaiLieu);
        }

        // GET: BoSungTaiLieux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoSungTaiLieu boSungTaiLieu = db.BoSungTaiLieus.Find(id);
            if (boSungTaiLieu == null)
            {
                return HttpNotFound();
            }
            return View(boSungTaiLieu);
        }

        // POST: BoSungTaiLieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BoSungTaiLieu boSungTaiLieu = db.BoSungTaiLieus.Find(id);
            db.BoSungTaiLieus.Remove(boSungTaiLieu);
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
