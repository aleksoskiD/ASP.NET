using Domain.Entities;
using Domain.Interfaces;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskMenagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectController : Controller
    {
        private IProjectRepository _projectRepository = new ProjectRepository();
        private ICustomerRepository _customerRepository = new CustomerRepository();
        private ITaskRepository _taskRepository = new TaskRepository();
        // GET: Project

        public ActionResult Index()
        {
            ViewBag.TasksCount = _taskRepository.GetAll();
            ViewBag.CustomerId = new SelectList(_customerRepository.GetAll().Where(x => x.IsActive == true), "ID", "Email");
            ViewBag.CustomerIdForEdit = new SelectList(_customerRepository.GetAll().Where(x => x.IsActive == true), "ID", "Email");

            return View(_projectRepository.GetAll());
        }

        [HttpPost]
        public JsonResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                if (_projectRepository.Create(project))
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if(_projectRepository.Delete(id))
                return RedirectToAction("Index");

            ViewBag.ErrorProject = "Error while deleting project";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult UpdateProject(int id, string projectName, int customerId, bool isActive)
        {
            Project project = new Project
            {
                ID = id,
                Name = projectName,
                CustomerId = customerId,
                IsActive = isActive
            };
            if (ModelState.IsValid)
            {
                if(_projectRepository.Update(project))
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false });
        }
    }
}