using HotelWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelWeb.Models
{
    public class SelectReservationModel
    {
        [Required(ErrorMessage = "Избери Од датум!")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Избери До датум!")]
        public string ToDate { get; set; }


        public IList<Room> Rooms { get; set; }

        [Required(ErrorMessage="Избери соба за резервација!")]
        public int RoomId { get; set; }



    }
}