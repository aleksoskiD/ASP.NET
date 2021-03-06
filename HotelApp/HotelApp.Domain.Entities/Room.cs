﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class Room
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsActive { get; set; }
        public bool IsReserved { get; set; }

        public virtual int FloorId { get; set; } // foreign key
        public virtual Floor Floor { get; set; }
    }

    public enum RoomType
    {
        singleRoom = 1,
        doubleRoom = 2,
        doubleLargeRoom = 3
    }
}
