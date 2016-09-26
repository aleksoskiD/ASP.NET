using HotelWeb.DAL.Repositories;
using HotelWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HotelWeb.Controllers
{
   // [Authorize]
    public class ReservationController : Controller
    {
        private UserRepository userRepositoy;
        private ReservationRepository reservationRepositoy;
        private RoomsRepository roomsRepositoy;

        public ReservationController()
        {
            this.userRepositoy = new UserRepository(new DAL.Context.HotelContext());
            this.reservationRepositoy = new ReservationRepository(new DAL.Context.HotelContext());
            this.roomsRepositoy = new RoomsRepository(new DAL.Context.HotelContext());
        }


        //
        // GET: /Reservation/Select
        public ActionResult Select(int? id)
        {
            var model = new SelectReservationModel();
            model.Rooms = this.roomsRepositoy.GetRooms();

            if (id.HasValue)
            {
                model.RoomId = id.Value;
            }

            return View("CreateReservation", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select(SelectReservationModel model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Внесете валидни податоци!");
                model.Rooms = this.roomsRepositoy.GetRooms();
                return View(model);
            }

            bool isCreated = this.reservationRepositoy.CreateReservation(new Guid(User.Identity.GetUserId()), model.RoomId, model.FromDate, model.ToDate);

            if (isCreated)
            {
                // reservation is successfull - > do redirect 
                return RedirectToAction("MyReservations");
            }

            ModelState.AddModelError("", "Внесете валидни податоци!");

            // error, redisplay the form
            model.Rooms = this.roomsRepositoy.GetRooms();
            return View("CreateReservation", model);
        }


        //
        // GET: /Reservation/MyReservations
        public ActionResult MyReservations()
        {

            var model = new ReservationsModel();
            model.Items = this.reservationRepositoy.GetReservations(new Guid(User.Identity.GetUserId()));

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            var result = this.reservationRepositoy.DeleteReservation(id);

            return RedirectToAction("MyReservations");
        }


        #region Helpers

            private IEnumerable<SelectListItem> GetRoomsListItemsForReservation()
            {
                return this.roomsRepositoy.GetRooms().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            }

        

        #endregion

    }
}