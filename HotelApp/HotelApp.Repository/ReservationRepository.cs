using HotelApp.Domain.Entities;
using HotelApp.Domain.Entities.ViewModels;
using HotelApp.Domain.Interfaces;
using HotelApp.Models;
using HotelApp.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private ApplicationDbContext appDb = new ApplicationDbContext();
        private AdminRepository _adminRepo = new AdminRepository();

        // for reservations
        public List<ReservationViewModel> GetAllReservations()
        {           
            List<ReservationViewModel> formatedReservations = new List<ReservationViewModel>();
            var reservations = appDb.Reservations.OrderByDescending(x=>x.ID).ToList();
            foreach (var item in reservations)
            {
                ApplicationUser guest = _adminRepo.GetGuestById(item.GuestId);
                item.Guest = guest;
                guest = null;
            }
            formatedReservations = reservations.ClientReservations();
            return formatedReservations;
        }

        public List<ReservationViewModel> GetReservationsForGuest(string id)
        {

            List<ReservationViewModel> formatedReservations = new List<ReservationViewModel>();
            var reservations = appDb.Reservations.Where(x => x.GuestId == id && x.EndDate > DateTime.Now).OrderByDescending(x=>x.DateCreated).ToList();

            formatedReservations = reservations.ClientReservations();
            return formatedReservations;
        }

        public Reservation GetReservationById(int id)
        {
            return appDb.Reservations.FirstOrDefault(x => x.ID == id);
        }

        public bool CreateReservation(Reservation reservation)
        {
            // use da se proveri dali dateCreated e pomaloo od startDate
            if (reservation != null && reservation.GuestId != null && _adminRepo.ReserveRoom(reservation.RoomId) == true && reservation.StartDate < reservation.EndDate)
            {
                reservation.DateCreated = DateTime.Now;
                reservation.Status = BookingStatus.pending;
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

        public bool ConfirmReservation(int id)
        {
            var dbReservation = GetReservationById(id);
            var confirmedReservation = dbReservation;
            if (dbReservation != null)
            {
                confirmedReservation.Status = BookingStatus.approved;
                appDb.Entry(dbReservation).CurrentValues.SetValues(confirmedReservation);
                appDb.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteReservation(int id)
        {
            var reservation = GetReservationById(id);
            if (reservation != null && _adminRepo.CancelRoom(reservation.RoomId)) 
            {
                reservation.Status = BookingStatus.notApproved;
                appDb.Entry(reservation).State = EntityState.Modified;
                appDb.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CancelReservation(int id)
        {
            var reservation = GetReservationById(id);
            if (reservation != null && _adminRepo.CancelRoom(reservation.RoomId))
            {
                reservation.Status = BookingStatus.cancelled;
                appDb.Entry(reservation).State = EntityState.Modified;
                appDb.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
