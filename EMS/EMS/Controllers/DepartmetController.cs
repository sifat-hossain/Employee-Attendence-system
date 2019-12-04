using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMS.Models;

namespace EMS.Controllers
{
    public class DepartmetController : Controller
    {
        private EMSEntities db = new EMSEntities();

        public ActionResult Index()
        {
            if (Session["name"].ToString() == "admin")
            {
                return View(db.Departments.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Create()
        {
            if (Session["name"].ToString() == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentId,DepartmentName")] Department department)
        {
            if(ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }
        public ActionResult Edit(int? id)
        {
            if (Session["name"].ToString() == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department department = db.Departments.Find(id);
                if (department == null)
                {
                    return HttpNotFound();
                }
                return View(department);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentId,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }
        [HttpPost]
        public JsonResult CheckDepartment(string department)
        {
            bool value = db.Departments.ToList().Exists(x => x.DepartmentName.ToLower().Equals(department.ToLower()));

            return Json(value);
        }
    }
}