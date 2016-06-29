using Domain.Interfaces;
using Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskMenagementSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()

        {
            ViewBag.Desc = "Welcome to Home Page of my project";

            return View();
        }
    }
}