using HotelApp.Domain.Interfaces;
using HotelApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelApp.Controllers
{
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

        public JsonResult DeleteReservation(int id)
        {
            if (ModelState.IsValid)
            {
                if (_reservationRepository.DeleteReservation(id))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

    }
}