using ASP.NET.Homework1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Homework1.Controllers
{
    public static class Counter{
        public static int counter = 0;
    }

    public static class Log
    {
        public static int log = 0;
    }
    public class AccountController : Controller
    {
        Database db = new Database();
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Count = Counter.counter;
            ViewBag.Log = Log.log;

            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var users = db.GetUsers();

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == username && users[i].Password == password)
                {
                    Counter.counter = 0;
                    return RedirectToAction("Index", "Equipment");
                }
            }
            Counter.counter++;
            if (Counter.counter > 5)
            {
                Counter.counter = 1;
            };
            Log.log = DateTime.Now.Minute;
           //proba
            var prom = Log.log;

            return RedirectToAction("Login");
        }
	}
}