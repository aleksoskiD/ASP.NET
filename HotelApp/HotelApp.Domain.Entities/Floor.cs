using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Floor
    {
        public int ID { get; set; }
        public int FloorNo { get; set; }
        public int NumberOfRooms { get; set; }
        public bool IsActive { get; set; }

        public List<Room> Rooms { get; set; } // many
    }
}
