using HotelApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Reservation
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; }
        
        public virtual int RoomId { get; set; } // foreign key
        public virtual Room Room { get; set; }

        public virtual string GuestId { get; set; } //foreign key
        public virtual ApplicationUser Guest { get; set; }
    }

    public enum BookingStatus
    {
        pending,
        approved,
        notApproved,
        cancelled
    }
}
