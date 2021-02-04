using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VillageBuildingReservation.Models
{
    public class Block
    {
        public int Id { get; set; }

        public string Key { get; set; }
        [DisplayName("المنشأة")]

        public string Name  { get; set; }

        public string FullBlockName
        {
            get { return this.Key + " : " + this.Name + " "; }
        }
        public string Coordinates { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

    }
}