using HotelApp.Domain.Entities;
using HotelApp.Domain.Interfaces;
using HotelApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using HotelApp.Models;
using HotelApp.Domain.Entities.ViewModels;

namespace HotelApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IAdminRepository _adminRepository = new AdminRepository();
        private IReservationRepository _reservationRepository = new ReservationRepository();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");
            ViewBag.TotalFloors = _adminRepository.GetAllFloors().Count;
            ViewBag.TotalRooms = _adminRepository.GetAllRooms().Count;
            ViewBag.TotalGuests = _adminRepository.GetAllGuests().Count;

            var reservations = _reservationRepository.GetAllReservations();
            ViewBag.TotalReservations = reservations.Count;
            ViewBag.Reservations = reservations.Where(x=>x.Status == BookingStatus.approved).Take(5);
            ViewBag.PendingReserations = reservations.Where(x => x.Status == BookingStatus.pending).Take(5);
            return View();
        }

        // FLOORS
        public ActionResult Floors()
        {
            ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");
            return View(_adminRepository.GetAllFloors());
        }

        [HttpPost]
        public JsonResult CreateFloor(Floor floor)
        {
            if (ModelState.IsValid)
            {
                FloorViewModel f = _adminRepository.CreateFloor(floor);

                if (f != null)
                {
                    return Json( f, JsonRequestBehavior.AllowGet);
                }
            }
            return Json( false , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeactivateFloor(int FloorId)
        {
            if (ModelState.IsValid)
            {
                FloorViewModel f = _adminRepository.DeactivateFloor(FloorId);
                if (f != null)
                {
                    return Json( f, JsonRequestBehavior.AllowGet);
                }
            }
            return Json( null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivateFloor(int FloorId)
        {
            if (ModelState.IsValid)
            {
                FloorViewModel f = _adminRepository.ActivateFloor(FloorId);
                if (f != null)
                {
                    return Json(f, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateFloor(Floor floor)
        {
            FloorViewModel f = _adminRepository.UpdateFloor(floor);
            if (ModelState.IsValid)
            {
                if (f != null)
                {
                    return Json(new { success = f }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json( false , JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFloor(int id)
        {
            if (ModelState.IsValid)
            {
                var floor = _adminRepository.GetFloorById(id);
                if (floor != null)
                {
                    return Json(new{
                        floorId = floor.ID,
                        floorNo = floor.FloorNo,
                        numOfRooms = floor.NumberOfRooms,
                        isActive = floor.IsActive
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }


        // ROOMS
        public ActionResult Rooms(int? id, string sort)
        {
            List<Room> rooms;
            if (id != null)
            {
                /// for creating room
                ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");

                switch (sort)
                {
                    case "free":
                        rooms= _adminRepository.AllFreeRooms().Where(x => x.FloorId == id).ToList();
                        break;
                    case "reserved":
                        rooms = _adminRepository.AllReservedRooms().Where(x => x.FloorId == id).ToList();
                        break;
                    case "active":
                        rooms = _adminRepository.AllActiveRooms().Where(x => x.FloorId == id).ToList();
                        break;
                    case "inactive":
                        rooms = _adminRepository.AllInactiveRooms().Where(x => x.FloorId == id).ToList();
                        break;
                    default:
                        rooms = _adminRepository.GetAllRooms().Where(x => x.FloorId == id).ToList();
                        break;
                }
                return View(rooms);
            }
            /// for creating room
            ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");

            switch (sort)
            {
                case "free":
                    rooms = _adminRepository.AllFreeRooms().ToList();
                    break;
                case "reserved":
                    rooms = _adminRepository.AllReservedRooms().ToList();
                    break;
                case "active":
                    rooms = _adminRepository.AllActiveRooms().ToList();
                    break;
                case "inactive":
                    rooms = _adminRepository.AllInactiveRooms().ToList();
                    break;
                default:
                    rooms = _adminRepository.GetAllRooms().ToList();
                    break;
            }
            return View(rooms);
        }

        [HttpPost]
        public JsonResult CreateRoom(Room room, RoomType type, int quantity)
        {
            if (ModelState.IsValid)
            {
                room.RoomType = type;
                if (_adminRepository.CreateRoom(room, quantity))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json( new { success = false}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeactivateRoom(int RoomId)
        {
            if (ModelState.IsValid)
            {
                if (_adminRepository.DeactivateRoom(RoomId))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivateRoom(int RoomId)
        {
            if (ModelState.IsValid)
            {
                if (_adminRepository.ActivateRoom(RoomId))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoom(int id)
        {
            if (ModelState.IsValid)
            {
                var room = _adminRepository.GetRoomById(id);
                if ( room != null)
                {
                    return Json(new {
                        roomId = room.ID,
                        floorId = room.FloorId,
                        roomType = room.RoomType.ToString(),
                        isActive = room.IsActive,
                        isReserved = room.IsReserved,
                        description = room.Description
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
               // room.RoomType = type;
                if (_adminRepository.UpdateRoom(room))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }



        // RESERVATIONS
        public ActionResult Reservations(string id, string sort)
        {
            List<ReservationViewModel> reservations;
            if(id != null )
            {
                switch (sort)
                {
                    case "approved":
                        reservations = _reservationRepository.GetReservationsForGuest(id).Where(x => x.Status == BookingStatus.approved).ToList();
                        break;
                    case "notApproved":
                        reservations = _reservationRepository.GetReservationsForGuest(id).Where(x => x.Status == BookingStatus.notApproved).ToList();
                        break;
                    case "pending":
                        reservations = _reservationRepository.GetReservationsForGuest(id).Where(x => x.Status == BookingStatus.pending).ToList();
                        break;
                    case "cancelled":
                        reservations = _reservationRepository.GetReservationsForGuest(id).Where(x => x.Status == BookingStatus.cancelled).ToList();
                        break;
                    default:
                        reservations = _reservationRepository.GetReservationsForGuest(id);
                        break;
                }
                ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");
                return View(reservations);
            }
            else
            {
                switch (sort)
                {
                    case "approved":
                        reservations = _reservationRepository.GetAllReservations().Where(x => x.Status == BookingStatus.approved).ToList();
                        break;
                    case "notApproved":
                        reservations = _reservationRepository.GetAllReservations().Where(x => x.Status == BookingStatus.notApproved).ToList();
                        break;
                    case "pending":
                        reservations = _reservationRepository.GetAllReservations().Where(x => x.Status == BookingStatus.pending).ToList();
                        break;
                    case "cancelled":
                        reservations = _reservationRepository.GetAllReservations().Where(x => x.Status == BookingStatus.cancelled).ToList();
                        break;
                    default:
                        reservations = _reservationRepository.GetAllReservations();
                        break;
                }
                ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");
                return View(reservations);
            }
        }

        [HttpPost]
        public JsonResult ConfirmReservation(int id)
        {
            if (ModelState.IsValid)
            {
                if (_reservationRepository.ConfirmReservation(id))
                {
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteReservation(int id)
        {
            if (ModelState.IsValid)
            {
                if (_reservationRepository.DeleteReservation(id))
                {
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false}, JsonRequestBehavior.AllowGet);
        }

        // GUESTS
        public ActionResult Guests()
        {
            ViewBag.FloorId = new SelectList(_adminRepository.GetAllFloors(), "ID", "FloorNo");
            var guests = _adminRepository.GetAllGuests();

            return View(guests);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Guest/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // POST: /Guest/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Admin");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Admin");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion
    }
}