using HotelWeb.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Entities
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public RoomType RoomType { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Descirption { get; set; }
        public int MaxPersons { get; set; }
        public string ImagePreviewUrl { get; set; }
    }
}