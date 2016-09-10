using HotelApp.Domain.Entities;
using HotelApp.Domain.Entities.ViewModels;
using HotelApp.Domain.Interfaces;
using HotelApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelApp.Controllers
{
    [Authorize(Roles ="Guest")]
    public class ReservationController : Controller
    {
        private IAdminRepository _adminRepository = new AdminRepository();
        private IReservationRepository _reservationRepository = new ReservationRepository();

        // GET: Reservation
        public ActionResult Index(string id)
        {
            ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");
            var c = _reservationRepository.GetReservationsForGuest(id);
            return View(c);
        }

        public JsonResult CancelReservation(int id)
        {
            if (ModelState.IsValid)
            {
                if (_reservationRepository.CancelReservation(id))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateReservation(string id, string roomType)
        {
            if (id!= null)
            {
                ViewBag.GuesId = id;
            }
            if (roomType != null)
            {
                ViewBag.RoomType = TypeOfRoom(roomType);
            }
            ViewBag.RoomId = new SelectList(_adminRepository.GetAllRooms(), "ID", "ID");
            return View();
        }

        [HttpPost]
        public JsonResult CreateReservation(string GuestId, string StartDate, string EndDate, RoomType RoomType)
        {
            if (ModelState.IsValid)
            {
                var room = _adminRepository.GetRoomByType(RoomType);
                if (GuestId != null && StartDate != null && EndDate != null && room != null)
                {
                    Reservation reservation = new Reservation()
                    {
                        GuestId = GuestId,
                        RoomId = room.ID,
                        DateCreated = DateTime.Now,
                        StartDate = DateTime.Parse(StartDate),
                        EndDate = DateTime.Parse(EndDate),
                        Status = BookingStatus.pending
                    };
                    if (_reservationRepository.CreateReservation(reservation))
                    {
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //_reservationRepository.CreateReservation(res);
            // return RedirectToAction("Index");        
        }


        private RoomType TypeOfRoom(string roomType)
        {
            RoomType type = RoomType.singleRoom;
            switch (roomType)
            {
                case "single":
                    type = RoomType.singleRoom;
                    break;
                case "double":
                    type = RoomType.doubleRoom;
                    break;
                case "doubleLarge":
                    type = RoomType.doubleLargeRoom;
                    break;
                default:
                    break;
            }
            return type;
        }
    }
}