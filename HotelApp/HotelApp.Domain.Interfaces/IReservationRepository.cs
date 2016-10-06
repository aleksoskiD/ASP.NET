using HotelApp.Domain.Entities;
using HotelApp.Domain.Entities.ViewModels;
using HotelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Interfaces
{
    public interface IReservationRepository
    {
        // for reservations
        List<ReservationViewModel> GetAllReservations();
        List<ReservationViewModel> GetReservationsForGuest(string id);
        List<ReservationViewModel> GetReservationsForGuestByDate(string id, DateTime date);
        Reservation GetReservationById(int id);
        bool CreateReservation(Reservation reservation);
        bool UpdateReservation(Reservation reservation);
        bool ConfirmReservation(int id);
        bool DeleteReservation(int id);
        bool CancelReservation(int id);
    }
}
