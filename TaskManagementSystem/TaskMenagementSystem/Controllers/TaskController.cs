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
    [Authorize(Roles = "Admin, User")]
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository = new TaskRepository();
        private IUserRepository _userRepository = new UserRepository();
        private IProjectRepository _projectRepository = new ProjectRepository();


        public ActionResult Index(int id)
        {
            ViewBag.ProjectId = id;
            //for displaying project name
            ViewBag.ProjectName = _projectRepository.GetById(id).Name;

            //dropdown list for creating task
            ViewBag.UserId = new SelectList(_userRepository.GetAll().Where(x => x.Role != "Admin"), "ID", "Email");

            var result = _taskRepository.GetAll().Where(x => x.ProjectId == id).ToList();
            return View(result);
        }


        // When User is logged give his tasks
        public ActionResult UserTask(string email)
        {
            ViewBag.UserEmail = email;
            int user = _userRepository.GetAll().FirstOrDefault(x => x.Email == email).ID;
            ViewBag.UserId = user;
            //ViewBag.UserId = new SelectList(_userRepository.GetAll().Where(x => x.Role != "Admin"), "ID", "Email");
           // int nes = ViewBag.UserId;
            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "ID", "Name");


            var result = _taskRepository.GetAll().Where(x => x.User.Email == email).ToList();
            return View(result);
        }

        [HttpPost]
        public JsonResult Create(string name, string description, int EstimatedHours, DateTime? endDateTime, TaskType tasktype, TaskStatus taskStatus, int userId, int ProjectId, bool IsActive)
        {
            Task task = new Task
            {
                Name = name,
                Description = description,
                EstimatedHours = EstimatedHours,
                EndDateTime = endDateTime,
                Type = tasktype,
                Status = taskStatus,
                UserId = userId,
                ProjectId = ProjectId,
                IsActive = IsActive
            };
            if (ModelState.IsValid)
            {
                if (_taskRepository.Create(task))
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTask(int id, string status)
        {
            if (ModelState.IsValid)
            {
                if (_taskRepository.UpdateTaskStatus(id, status))
                    return Json(new { success = true}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTask(Task task, TaskType tasktype, TaskStatus taskStatus)
        {
            task.Status = taskStatus;
            task.Type = tasktype;
            if (ModelState.IsValid)
            {
                if (_taskRepository.Create(task))
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}