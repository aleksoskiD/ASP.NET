using HotelApp.Domain.Entities;
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
        List<Reservation> GetAllReservations();
        List<Reservation> GetReservationsForGuest(string id);
        Reservation GetReservationById(int id);
        bool CreateReservation(Reservation reservation);
        bool UpdateReservation(Reservation reservation);
        bool DeleteReservation(int id);
    }
}
