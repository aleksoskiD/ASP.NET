using Domain.Entities;
using Domain.Interfaces;
using Repository;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskMenagementSystem.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository = new TaskRepository();
        private IUserRepository _userRepository = new UserRepository();
        private IProjectRepository _projectRepository = new ProjectRepository();


        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(int id)
        {
            ViewBag.ProjectId = id;
            ViewBag.ProjectName = _projectRepository.GetById(id).Name;

            var result = _taskRepository.GetAll().Where(x => x.ProjectId == id).ToList();
            return View(result);
        }


        // When User is logged give his tasks
        [Authorize(Roles = "User, Admin")]
        public ActionResult UserTask(string email)
        {
            ViewBag.UserEmail = email;
            var result = _taskRepository.GetAll().Where(x => x.User.Email == email).ToList();
            return View(result);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Create(int? id, string email)
        {
            ViewBag.ProjectName= id; // if admin create task 
            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "ID", "Name");

            ViewBag.UserId = new SelectList(_userRepository.GetAll().Where(x => x.Role != "Admin"), "ID", "Email");

            // if user create task
            if(email != null)
            {
                ViewBag.UserEmail = _userRepository.GetAll().FirstOrDefault(x => x.Email == email).ID;
            }
            
            return View();
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                if (_taskRepository.Create(task))
                    return RedirectToAction("Index",  new { id = task.ProjectId});
            }
            ViewBag.ProjectName = task.ProjectId;
            ViewBag.UserId = new SelectList(_userRepository.GetAll(), "ID", "Email");
            ViewBag.ErrorMessage = "Error";
            return View();
        }
    }
}