using HotelWeb.DAL.Repositories;
using HotelWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelWeb.Controllers
{
    public class HomeController : Controller
    {
        private IRoomsRepository roomRepository;
        public HomeController()
        {
            this.roomRepository = new RoomsRepository(new HotelWeb.DAL.Context.HotelContext());
        }



        public ActionResult Index()
        {            
            // get rooms
            var rooms = new RoomsModel();
            rooms.Items = this.roomRepository.GetRooms();

            // put rooms into view bag
            ViewBag.Rooms = rooms;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}