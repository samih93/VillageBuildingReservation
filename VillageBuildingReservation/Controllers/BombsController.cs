using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VillageBuildingReservation.Models;

namespace VillageBuildingReservation.Controllers
{
    public class BombsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bombs
        public ActionResult Index()
        {
            return View(db.Bombs.ToList());
        }

        // GET: Bombs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bombs bombs = db.Bombs.Find(id);
            if (bombs == null)
            {
                return HttpNotFound();
            }
            return View(bombs);
        }

        // GET: Bombs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bombs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Bombs bombs)
        {
            if (ModelState.IsValid)
            {
                db.Bombs.Add(bombs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bombs);
        }

        // GET: Bombs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bombs bombs = db.Bombs.Find(id);
            if (bombs == null)
            {
                return HttpNotFound();
            }
            return View(bombs);
        }

        // POST: Bombs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Bombs bombs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bombs).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = MessagingSystem.AddMessage("تم تعديل العتاد بنجاح", "success");
                return RedirectToAction("Index");
            }
            return View(bombs);
        }

        // GET: Bombs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bombs bombs = db.Bombs.Find(id);
            if (bombs == null)
            {
                return HttpNotFound();
            }
            return View(bombs);
        }

        // POST: Bombs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bombs bombs = db.Bombs.Find(id);
            db.Bombs.Remove(bombs);
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
