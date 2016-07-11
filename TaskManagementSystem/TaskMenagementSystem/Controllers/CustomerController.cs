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
    [Authorize(Roles ="Admin")]
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepository = new CustomerRepository();
        
        // GET: Customer
        public ActionResult Index()
        {
            return View(_customerRepository.GetAll());
        }

        [HttpPost]
        public JsonResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (_customerRepository.Create(customer))
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}