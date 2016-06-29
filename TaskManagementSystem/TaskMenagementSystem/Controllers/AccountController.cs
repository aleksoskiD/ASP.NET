using Domain.Entities;
using Domain.Interfaces;
using Repository;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TaskMenagementSystem.Controllers
{
    public class AccountController : Controller
    {
        Database db = new Database();
        IUserRepository _userRepository = new UserRepository();
        ITaskRepository _taskRepository = new TaskRepository();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //var cnt = _taskRepository.GetAll().OrderBy(x => x.UserId).ToList();
            return View(_userRepository.GetAll().Where(x=>x.Role != "Admin"));
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.Register(user))
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            var dbUser = db.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            var role = "";
            if (dbUser != null)
            {
                role = db.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password).Role;
            }

            if (dbUser == null || dbUser.IsActive == false)
            {
                ViewBag.Msg = "Invalid User or the User does not exist!";
                return View();
            }
            else
            {

                if (role.Equals("User")) { 
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("UserTask", "Task", new {email = user.Email});
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Project");
                }
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Activate(int id)
        {
            if(_userRepository.Delete(id))
                return RedirectToAction("Index");

            return RedirectToAction("Index");
        }
    }
}