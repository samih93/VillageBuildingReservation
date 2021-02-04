using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillageBuildingReservation.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Block> Blocks { get; set; }
    }
}