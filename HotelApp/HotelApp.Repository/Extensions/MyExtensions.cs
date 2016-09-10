using HotelApp.Domain.Entities;
using HotelApp.Domain.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Repository.Extensions
{
    public static class MyExtensions
    {
        public static List<ReservationViewModel> ClientReservations(this List<Reservation> reservations)
        {
            List<ReservationViewModel> formatedReservations = new List<ReservationViewModel>();
            for (int i = 0; i < reservations.Count; i++)
            {
                ReservationViewModel model = new ReservationViewModel();
                model.ID = reservations[i].ID;
                model.DateCreated = reservations[i].DateCreated.ToShortDateString();
                model.StartDate = reservations[i].StartDate.ToShortDateString();
                model.EndDate = reservations[i].EndDate.ToShortDateString();
                model.Status = reservations[i].Status;
                model.RoomType = reservations[i].Room.RoomType;
                model.Guest_Id = reservations[i].GuestId;
                model.GuestName = reservations[i].Guest.UserName;

                formatedReservations.Add(model);
                model = null;
            }
            return formatedReservations;
        }
    }
}
