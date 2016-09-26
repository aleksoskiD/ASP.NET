using HotelWeb.DAL.Context;
using HotelWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Repositories
{
    public interface IReservationRepository
    {
        bool CreateReservation(Guid userId, int roomId, string fromDate, string toDate);

        IList<Reservation> GetReservations();
        IList<Reservation> GetReservations(Guid userId);

        bool DeleteReservation(int id);
    }

    public class ReservationRepository : IReservationRepository
    {
        private HotelContext context;
        public ReservationRepository(HotelContext context)
        {
            this.context = context;         
        }



        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <returns>Reservations list</returns>
        public IList<Reservation> GetReservations()
        {
            return context.Reservations.ToList();
        }


        /// <summary>
        /// Get all resevrations for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Reservations list for hte user</returns>
        public IList<Reservation> GetReservations(Guid userId)
        {
            return context.Reservations.Where(x => x.UserId == userId).ToList();
        }


        /// <summary>
        /// Ceate a new reservation in the system
        /// </summary>
        /// <param name="userId">Customer Id</param>
        /// <param name="roomId">Reservation room Id</param>
        /// <param name="fromDate">Reservation start data</param>
        /// <param name="toDate">Reservation end date</param>
        /// <returns>True/False</returns>
        public bool CreateReservation(Guid userId, int roomId, string fromDate, string toDate)
        {
            try
            {
                
                var reservation = new Reservation();
                reservation.CreatedOn = DateTime.Now;
                reservation.UserId = userId;
                reservation.RoomId = roomId;
                reservation.BookingStatus = Entities.Enums.BookingStatus.Pending;

                try
                {
                    var FromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var ToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (FromDate >= ToDate)
                    {
                        return false;
                    }
                    
                    reservation.FromDate = FromDate;
                    reservation.ToDate = ToDate;
                }
                catch
                {
                    return false;
                }


                // add the reservation record
                context.Reservations.Add(reservation);

                // save db changes
                return context.SaveChanges() > 0; // returns true if changes are successful
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Dleete reservation
        /// </summary>
        /// <param name="id">reservation id</param>
        /// <returns>True/False</returns>
        public bool DeleteReservation(int id)
        {
            try
            {
                var r = context.Reservations.Where(x => x.Id == id).FirstOrDefault();
                if(r != null)
                {
                    context.Reservations.Remove(r);
                    return context.SaveChanges() > 0;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


    }
}