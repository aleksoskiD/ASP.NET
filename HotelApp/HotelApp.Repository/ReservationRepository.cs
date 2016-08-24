using HotelApp.Domain.Entities;
using HotelApp.Domain.Interfaces;
using HotelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // for reservations
        public List<Reservation> GetAllReservations()
        {
            return appDb.Reservations.Where(x => x.IsActive == true).ToList();
        }

        public List<Reservation> GetReservationsForGuest(string id)
        {
            return appDb.Reservations.Where(x => x.Guest.Id == id).ToList();
        }

        public Reservation GetReservationById(int id)
        {
            return appDb.Reservations.FirstOrDefault(x => x.ID == id);
        }

        public bool CreateReservation(Reservation reservation)
        {
            if (reservation != null)
            {
                reservation.DateCreated = DateTime.Now;
                appDb.Reservations.Add(reservation);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateReservation(Reservation reservation)
        {
            var dbReservation = GetReservationById(reservation.ID);

            if (dbReservation != null)
            {
                reservation.DateCreated = dbReservation.DateCreated;
                appDb.Entry(dbReservation).CurrentValues.SetValues(reservation);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteReservation(int id)
        {
            var reservation = GetReservationById(id);
            if (reservation != null)
            {
                appDb.Reservations.Remove(reservation);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
