using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities.ViewModels
{
    public class ReservationViewModel
    {
        public int ID { get; set; }
        public string DateCreated { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public BookingStatus Status { get; set; }

        public RoomType RoomType { get; set; }

        public string Guest_Id { get; set; } //foreign key
        public string GuestName { get; set; }
    }
}
