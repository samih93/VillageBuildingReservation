using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VillageBuildingReservation.Models;
using VillageBuildingReservation;
using System.Web.UI.WebControls;

namespace VillageBuildingReservation.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,admin ,general")]
        public ActionResult Index()
        {
            DateTime CurrentDate = DateTime.Now;
            ReservationVM VM = new ReservationVM();
            VM.Year = CurrentDate.Year;
            VM.Month = CurrentDate.Month;
            VM.Reservations =  db.Reservations.Where(m => m.ReservationDate.Year == VM.Year && m.ReservationDate.Month == VM.Month).OrderBy(x => x.ReservationDate).Include(b=>b.Bombs).Include(b=>b.Blocks).ToList();
            VM.DDlYear = GlobalController.GenerateYearsDDL(VM.Year);
            VM.DDlMonth = GlobalController.GenerateMonthsDDL(VM.Month).ToList() ;
            return View(VM);
        }
        [HttpPost]
        [Authorize(Roles = "admin ,general")]
        public ActionResult Index(ReservationVM VM)
        {
            VM.DDlYear = GlobalController.GenerateYearsDDL(VM.Year);
            VM.DDlMonth = GlobalController.GenerateMonthsDDL(VM.Month).ToList();
            VM.Reservations = db.Reservations.Where(m => m.ReservationDate.Year == VM.Year && m.ReservationDate.Month == VM.Month).OrderBy(x => x.ReservationDate).Include(b => b.Bombs).Include(b => b.Blocks).ToList();
            
            return View(VM);
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            //var query = (from b in db.blocks
            //             join z in db.zones on b.zoneid equals z.id
            //             select new { b, z }).tolist();
            // ViewBag.listOfBlocks = new SelectList(db.Blocks.Include(Z=>Z.Zone), "Id", "FullBlockName");
            //List<SelectListItem> list  = new List<SelectListItem>();
            //list = (db.Blocks.Include(Z =>Z.Zone).Select(
            //    x => new SelectListItem()
            //    {
            //        Value = x.Id.ToString(),
            //        Text = x.Key + " : "+x.Name
            //    })).ToList() ;
           

            //foreach (ListItem listitem in list)
            //{
            //    Block block = db.Blocks.Find(long.Parse(listitem.Value));
            //    Zone zone = db.Zones.Find(block.ZoneId);
            //    if (zone != null)
            //    {
            //        listitem.Attributes.Add("optiongroup", zone.Name);
            //    }
            //}
            // select last record Id 
            int id = db.Reservations.Max(r => r.Id);
            // get the last reservation
            Reservation reservation = db.Reservations.Find(id);

            List<Block> Blocks = db.Blocks.Include(X => X.Zone).ToList();
            ViewBag.listOfBlocks = Blocks;
            ViewBag.listBombs = new MultiSelectList(db.Bombs, "Id", "Name", GetReservationBombsIds(reservation));
            return View(reservation);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Reservation reservation, List<int> listOfBlocksDDL, List<int> listOfBombsDDL)
        {
            DateTime t1, t2;

            if (ModelState.IsValid)
            {
                List<Block> Blocks = db.Blocks.Include(X => X.Zone).ToList();


                // check  list of blocks 
                //if (listOfBlocksDDL == null || listOfBlocksDDL.Count == 0)
                //{
                //    TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage(" المنشأة  "), "danger");
                //    ViewBag.listOfBlocks = Blocks;
                //    return View(reservation);
                //}

                /// Compare start and end time
                if (reservation.StartTime != null && reservation.EndTime != null)
                {
                    t1 = DateTime.Parse(reservation.ReservationDate.ToString("yyyy/MM/dd") + " " + reservation.StartTime.ToString() + ":00.000");
                    t2 = DateTime.Parse(reservation.ReservationDate.ToString("yyyy/MM/dd") + " " + reservation.EndTime.ToString() + ":00.000");

                    if (TimeSpan.Compare(t1.TimeOfDay, t2.TimeOfDay) == 1)
                    {
                        TempData["message"] = MessagingSystem.AddMessage("وقت البدأ يجب ان يكون اصغر من وقت الإنتهاء", "danger");
                        ViewBag.listOfBlocks = Blocks;
                        ViewBag.listBombs = new SelectList(db.Bombs, "Id", "Name");
                        return View(reservation);
                    }
                }
                 else
                {
                    TempData["message"] = MessagingSystem.AddMessage("يحب ادخال وقت البدء و الانتهاء", "danger");
                    ViewBag.listOfBlocks = Blocks;
                    ViewBag.listBombs = new SelectList(db.Bombs, "Id", "Name");
                    return View(reservation);
                }

                //adding selected blocks objects to reservation
                if (listOfBlocksDDL != null && listOfBlocksDDL.Count != 0)
                {
                    listOfBlocksDDL.ForEach(delegate (int BlockId)
                    {
                        reservation.Blocks.Add(db.Blocks.Find(BlockId));
                    });
                }

                //adding selected Bombs objects to reservation

                if (listOfBombsDDL != null && listOfBombsDDL.Count != 0)
                {
                    listOfBombsDDL.ForEach(delegate (int BombId)
                    {
                        reservation.Bombs.Add(db.Bombs.Find(BombId));
                    });
                    //barkya is needed set to true since bombs are checked
                    reservation.IsNeedBarkya = true;
                }

                db.Reservations.Add(reservation);
                db.SaveChanges();
                TempData["message"] = MessagingSystem.AddMessage("تم انشاء الحجز بنجاح", "success");

                return RedirectToAction("Index");
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            List<Block> Blocks = db.Blocks.Include(X => X.Zone).ToList();
            ViewBag.listOfBlocks = Blocks;
            ViewBag.listBombs = new MultiSelectList(db.Bombs, "Id", "Name", GetReservationBombsIds(reservation));


            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }



        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Reservation reservation, List<int> listOfBlocksDDL, List<int> listOfBombsDDL)
        {
            DateTime t1, t2;

            if (ModelState.IsValid)
            {
                List<Block> Blocks = db.Blocks.Include(X => X.Zone).ToList();
                ViewBag.listOfBlocks = Blocks;

                db.Entry(reservation).State = EntityState.Modified;
                //if (listOfBlocksDDL == null || listOfBlocksDDL.Count == 0)
                //{
                //    TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage(" المنشأة  "), "danger");
                //    return View(reservation);
                //}
                /// Compare start and end time
                if (reservation.StartTime != null && reservation.EndTime != null)
                {
                    t1 = DateTime.Parse(reservation.ReservationDate.ToString("yyyy/MM/dd") + " " + reservation.StartTime.ToString() + ":00.000");
                    t2 = DateTime.Parse(reservation.ReservationDate.ToString("yyyy/MM/dd") + " " + reservation.EndTime.ToString() + ":00.000");

                    if (TimeSpan.Compare(t1.TimeOfDay, t2.TimeOfDay) == 1)
                    {
                        TempData["message"] = MessagingSystem.AddMessage("وقت البدأ يجب ان يكون اصغر من وقت الإنتهاء", "danger");
                        ViewBag.listOfBlocks = Blocks;
                        ViewBag.listBombs = new SelectList(db.Bombs, "Id", "Name");
                        return View(reservation);
                    }
                }
                else
                {
                    TempData["message"] = MessagingSystem.AddMessage("يحب ادخال وقت البدء و الانتهاء", "danger");
                    ViewBag.listOfBlocks = Blocks;
                    ViewBag.listBombs = new SelectList(db.Bombs, "Id", "Name");
                    return View(reservation);
                }
                //clearing old reservationblock relation
                ClearReservationBlocks(reservation);
                ClearReservationBombs(reservation);
                //adding selected blocks objects to reservation
                if (listOfBlocksDDL != null && listOfBlocksDDL.Count != 0)
                {
                    listOfBlocksDDL.ForEach(delegate (int BlockId)
                    {
                        reservation.Blocks.Add(db.Blocks.Find(BlockId));
                    });
                }

                //adding selected Bombs objects to reservation
                if (listOfBombsDDL != null && listOfBombsDDL.Count != 0)
                {
                    listOfBombsDDL.ForEach(delegate (int BombId)
                    {
                        reservation.Bombs.Add(db.Bombs.Find(BombId));
                    });
                }
                db.SaveChanges();
                TempData["message"] = MessagingSystem.AddMessage("تم تعديل الحجز بنجاح", "success");

                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        private List<int> GetReservationBlock(Reservation res)
        {
            List<int> ids = new List<int>();
            List<int> data = db.Database.SqlQuery<int>($"SELECT  Block_Id FROM ReservationBlocks WHERE Reservation_Id = {res.Id}").ToList();
            return data;
        }
        private void ClearReservationBlocks(Reservation res)
        {
            db.Database.ExecuteSqlCommand($"DELETE FROM ReservationBlocks WHERE Reservation_Id = {res.Id}");
        }



        // GET: Reservations/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                TempData["message"] = MessagingSystem.AddMessage("الحجز غير موجود , تأكد من سجل الحجوزات", "danger");
                return View();
            }
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            TempData["message"] = MessagingSystem.AddMessage("تم حذف الحجز بنجاح", "success");
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
        // GET: Reservations
        public ActionResult VillageMap()
        {
            string reservationDate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime reservationDateDT = DateTime.Parse(reservationDate);
            ViewBag.reservationDate = reservationDate;

            ViewBag.hasNoBuildingReservations = db.Reservations.Where(m => m.ReservationDate == reservationDateDT && m.Blocks.Count == 0).Count();
            ViewBag.hasReservationNeedBarkya = db.Reservations.Where(m => m.ReservationDate == reservationDateDT && m.IsNeedBarkya == true).Count();
            ViewBag.blocks = db.Blocks.OrderBy(m => m.Id).ToList();
            return View(db.Reservations.ToList());
        }

        [HttpPost]
        public ActionResult VillageMap(string reservationDate)
        {
            ViewBag.reservationDate = DateTime.Parse(reservationDate).ToString("yyyy-MM-dd");
            DateTime reservationDateDT = DateTime.Parse(reservationDate);
            ViewBag.blocks = db.Blocks.OrderBy(m => m.Id).ToList();
            //check if there are reservations with no buildings
            ViewBag.hasNoBuildingReservations = db.Reservations.Where(m => m.ReservationDate == reservationDateDT && m.Blocks.Count == 0).Count();
            ViewBag.hasReservationNeedBarkya = db.Reservations.Where(m => m.ReservationDate == reservationDateDT && m.IsNeedBarkya == true).Count();

            return View(db.Reservations.ToList());
        }

        public List<int> GetReservationBombsIds(Reservation res)
        {
            List<int> ids = new List<int>();
            List<int> data = db.Database.SqlQuery<int>($"SELECT  Bombs_Id FROM BombsReservations WHERE Reservation_Id = {res.Id}").ToList();
            return data;
        }
        // check if reservation have
        public static bool IsEmptyReservation(Reservation reservation)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int count = db.Database.SqlQuery<int>($"SELECT  count(*)  FROM ReservationBlocks WHERE Reservation_Id = {reservation.Id}").Single();

            return count <= 0;
        }
        private void ClearReservationBombs(Reservation res)
        {
            db.Database.ExecuteSqlCommand($"DELETE FROM BombsReservations WHERE Reservation_Id = {res.Id}");
        }

        // 'Block F' outside village
        public static bool AllVillageIsReserved(Reservation reservation)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int res = db.Database.SqlQuery<int>($"select count(*) from ReservationBlocks WHERE Reservation_Id = {reservation.Id} and Block_Id <> 23" ).Single();
            return (res == 30);
        }

        public static bool BlockFIsReserved(Reservation reservation)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int res = db.Database.SqlQuery<int>($"select count(*) from ReservationBlocks WHERE Reservation_Id = {reservation.Id} and Block_Id = 23").Single();
            return (res == 1);

        }
        // return string of block in zone
        public static string GetBlocksZone(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string res = "";
            List<Block> Blocks = db.Blocks.Where(b =>b.ZoneId == Id).ToList();
            foreach (var item in Blocks)
            {
                res += item.Key.Trim() + " - ";
            }
            res = remove_last_caracter(res);

            return res;
        }
        // exemple A1-A2-A3-A4-A5-A6 -- > zone A is reserved
        //public static bool AllBlockInZoneIsReserved(Reservation reservation , Block block)
        //{
        //}
        public static string GetAllReservationBlocks_Of_Reservation(Reservation reservation)
        {
            string res = "";

            // check if all bock reserved and block F
            if (AllVillageIsReserved(reservation) && BlockFIsReserved(reservation))
            {
                res = "كامل القرية" + " - F";
            }
            else
            {
                if (AllVillageIsReserved(reservation))
                {
                    res = "كامل القرية";
                }
                else
                {
                    foreach (var block in reservation.Blocks.OrderBy(m=>m.ZoneId))
                    {
                        res += block.Key + " - ";
                    }
                    // replace zone A if all block in zone reserved
                    res = res.Replace(GetBlocksZone(1), "Zone A");
                    // replace zone B if all block in zone reserved
                    res = res.Replace(GetBlocksZone(2), "Zone B");
                    // replace zone C if all block in zone reserved
                    res = res.Replace(GetBlocksZone(3), "Zone C");
                    // replace zone D if all block in zone reserved
                    res = res.Replace(GetBlocksZone(4), "Zone D");
                    // replace zone E if all block in zone reserved
                    res = res.Replace(GetBlocksZone(5), "Zone E");


                    res = remove_last_caracter(res);

                }
            }
           
            return res;
        }
        public static string remove_last_caracter(string str)
        {
            if (str != null && str.Length > 0)
            {
                str = str.Substring(0, str.Length - 2);
            }
            return str;
        }

        public static string GetAllReservationBombs_Of_Reservation(Reservation reservation)
        {
            string res = "";

            foreach (var bombs in reservation.Bombs)
            {
                res += bombs.Name + " - ";
            }
            res = remove_last_caracter(res);

            return res;
        }

        public static List<Reservation> GetReservationsInMonth(DateTime date)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Reservations.Where(m=>m.ReservationDate.Year==date.Year && m.ReservationDate.Month == date.Month).OrderBy(x =>x.ReservationDate).ToList();
        }

        [HttpPost]
        public ActionResult ApproveAttendance(int id , bool flag)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                TempData["message"] = MessagingSystem.AddMessage("الحجز غير موجود", "danger");
                return RedirectToAction("Index");
            }

            reservation.IsAttended = flag;
            db.Entry(reservation).State = EntityState.Modified;
            db.SaveChanges();
            TempData["message"] = MessagingSystem.AddMessage("تم تأكيد الحضور", "danger");
            return RedirectToAction("Index");
        }
    }
}
