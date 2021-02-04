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
    public class BlocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blocks
        public ActionResult Index()
        {
            return View(db.Blocks.ToList());
        }

        // GET: Blocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Block block = db.Blocks.Find(id);
            if (block == null)
            {
                return HttpNotFound();
            }
            return View(block);
        }

        // GET: Blocks/Create
        public ActionResult Create()
        {
            ViewBag.ListOfZones = new SelectList(db.Zones, "Id", "Name");
            return View();
        }

        // POST: Blocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Block block, List<int> ListOfZones)
        {
            if (ModelState.IsValid)
            {
                db.Blocks.Add(block);
                db.Blocks.Find(block.Id).ZoneId = ListOfZones.First();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(block);
        }

        // GET: Blocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Block block = db.Blocks.Find(id);
            ViewBag.ListOfZones = new SelectList(db.Zones, "Id", "Name", block.ZoneId);

            if (block == null)
            {
                return HttpNotFound();
            }
            return View(block);
        }

        // POST: Blocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Block block, List<int> ListOfZonesDDL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(block).State = EntityState.Modified;
                db.Blocks.Find(block.Id).ZoneId = ListOfZonesDDL.First();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(block);
        }

        // GET: Blocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Block block = db.Blocks.Find(id);
            if (block == null)
            {
                return HttpNotFound();
            }
            return View(block);
        }

        // POST: Blocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Block block = db.Blocks.Find(id);
            db.Blocks.Remove(block);
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

        //checks if a block has a reservation (s) on a specific date, returns the list of reservations if true
        public static List<Reservation> BlockHasReservation(Block block, string date)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DateTime dt = DateTime.Parse(date);
            return db.Reservations.Where(m => m.Blocks.Select(b => b.Id).Contains(block.Id) && m.ReservationDate == dt).ToList();
        }

        //returns the block color in image map based on its state free / mixed / pure reserved / pure spare
        public static string BlockReservationsStateColor(Block block, string date)
        {
            List<Reservation> reservations = BlockHasReservation(block, date);
            bool hasConfirmedReservation = false, hasSpareReservation = false;
            string returnedStr = "";
            //if no reservations ( FREE) color GREEN
            if (reservations == null || reservations.Count == 0)
            {
                returnedStr = "00ff0a";
            }
            foreach (Reservation res in reservations)
            {
                //if at least one res is spare, we set boolean IsSpareReservation true
                if (res.IsSpareReservation)
                {
                    hasSpareReservation = true;
                }
                //if at least one res not spare (confirmed), we set boolean hasConfirmedReservation true
                else
                {
                    hasConfirmedReservation = true;
                }
            }
            //if hasConfirmedReservation true and hasSpareReservation true (MixedReservation) color BLUE
            if (hasSpareReservation && hasConfirmedReservation == true)
            {
                returnedStr = "0000ff";
            }
            else
            {
                //if hasConfirmedReservation true and hasSpareReservation false (PureReservation) color RED
                if (hasConfirmedReservation)
                {
                    return "ff0000";
                }
                //if hasConfirmedReservation false and hasSpareReservation true (PureSpare) color Orange
                else if (hasSpareReservation)
                {
                    returnedStr = "ffa500";
                }
            }

            return returnedStr;
        }
    }
}
