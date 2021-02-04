using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VillageBuildingReservation.Models
{
    public class ReservationVM
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public IEnumerable<SelectListItem> DDlYear { get; set; }
        public IEnumerable<SelectListItem> DDlMonth { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}