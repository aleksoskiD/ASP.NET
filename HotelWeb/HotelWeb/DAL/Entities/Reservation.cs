using HotelWeb.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Entities
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public BookingStatus BookingStatus { get; set; }

        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

    }
}