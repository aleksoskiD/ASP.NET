using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities.ViewModels
{
    public class FloorViewModel
    {
        public int ID { get; set; }
        public int FloorNo { get; set; }
        public bool IsActive { get; set; }
        public int TotalRooms { get; set; } // site
        public int Entered { get; set; } // vneseni
        public int ForEnter { get; set; } // za vnes
        public int Free { get; set; } // slobodni
        public int Reserved { get; set; } // rezervirani
        

        public virtual List<Room> Rooms { get; set; } // many
    }
}
