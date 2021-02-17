using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillageBuildingReservation.Models
{
    public class Reservation
    {
        public Reservation()
        {
            this.Blocks = new HashSet<Block>();
            this.Bombs = new HashSet<Bombs>();
        }
        public int Id { get; set; }

        [DisplayName("المستخدم")]
        [Required(ErrorMessage = "حقل المستخدم مطلوب")]
        public string ReservationName { get; set; }

        [DisplayName("تاريخ الحجز")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "حقل تاريخ الحجز مطلوب")]
        public DateTime ReservationDate { get; set; }
        

        [DisplayName("وقت البدء")]

        public string StartTime { get; set; }

        [DisplayName("وقت الإنتهاء")]
        public string EndTime { get; set; }
        [Required]
        [DisplayName("رقم المستند القاضي بالحجز")]
        public string DocumentNumber { get; set; }
        //[DisplayName("نشط؟")]
        //public bool Active { get; set; }
        [DisplayName("حجز احتياطي؟")]
        public bool IsSpareReservation { get; set; }
        [DisplayName(" بحاجة الى برقية؟")]
        public bool IsNeedBarkya { get; set; }
        [DisplayName("ملاحظات")]
        public string  Notes { get; set; }

        public bool? IsAttended { get; set; }

        public virtual ICollection<Block> Blocks { get; set; }
        public virtual ICollection<Bombs> Bombs { get; set; }

    }
}