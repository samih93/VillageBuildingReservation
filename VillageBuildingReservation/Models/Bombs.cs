using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VillageBuildingReservation.Models
{
    public class Bombs
    {
        public int Id { get; set; }
        [DisplayName("العتاد المستخدم")]

        public string Name { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

    }
}