using ASP.NET.Homework1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Homework1.Controllers
{
    //[Authorize]
    public class EquipmentController : Controller
    {
        Database db = new Database();

        public ActionResult Index(string query)
        {
            List<Equipment> equipment = db.GetEquipment();

            //for <span>assigness></span>
            if (equipment.Count() == 0)
            {
                ViewBag.Assigness = 0;

                int stf = ViewBag.Assigness;
            }

            for (int i = 0; i < equipment.Count; i++)
            {
                if (!string.IsNullOrEmpty(query) && equipment[i].AssignedTo.ToLower() == query.ToLower())
                {
                    var equipmentAssignedTo = equipment.Where(x => x.AssignedTo.ToLower().Equals(query.ToLower())).ToList();

                    return View(equipmentAssignedTo);
                }
            }

            if((!string.IsNullOrEmpty(query)))
                {

                var listString = query.Split().ToList();
                var joinedQuery = string.Join("", listString.ToArray());
                var equipmentSearched = equipment.Where(x => x.Name.ToLower().Contains(joinedQuery.ToLower().Trim())).ToList();

                if (equipmentSearched.Count == 0)
                {
                    return RedirectToAction("Error");
                }

                return View(equipmentSearched);
                }

            // for adding buttons
            List<string> distinct = equipment.Select(x => x.AssignedTo).Distinct().ToList();
            ViewData["Distinct"] = distinct;

            return View(equipment);
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Equipment item)
        {
            db.AddEquipment(item);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var equipmentItem = db.GetEquipment().FirstOrDefault(x => x.ID == id);

            return View(equipmentItem);
        }

        [HttpPost]
        public ActionResult Edit(Equipment item)
        {
            if (ModelState.IsValid)
            {
                db.EditEquipment(item);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var item = db.GetEquipment().FirstOrDefault(x => x.ID == id);

            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(Equipment item)
        {
            db.DeleteEquipment(item);

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var item = db.GetEquipment().FirstOrDefault(x => x.ID == id);

            return View(item);
        }

	}
}