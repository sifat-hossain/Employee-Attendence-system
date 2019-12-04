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
    public class EmployeeController : Controller
    {
        private EMSEntities db = new EMSEntities();

        public ActionResult Index()
        {
            if (Session["name"].ToString() == "admin")
            {
                return View(db.EmployeeInfoes.ToList());
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
                ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,EmployeeName,EmployeeGender,EmployeeEmail,EmployeePassword,EmployeePhone,DesignationId")] EmployeeInfo employee)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeInfoes.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", employee.DesignationId);
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (Session["name"].ToString() == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeeInfo employee = db.EmployeeInfoes.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", employee.DesignationId);
                return View(employee);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,EmployeeName,EmployeeGender,EmployeeEmail,EmployeePassword,EmployeePhone,DesignationId")] EmployeeInfo employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", employee.DesignationId);
            return View();
        }

        public ActionResult EmployeeProfile(int? id)
        {
            if (Session["id"] != null && Session["name"].ToString() == "employee")
            {
                id = Convert.ToInt16(Session["id"]);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeeInfo employeeInfo = db.EmployeeInfoes.Find(id);
                if (employeeInfo == null)
                {
                    return HttpNotFound();
                }
                return View(employeeInfo);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        public ActionResult SalaryById(int? id)
        {
            if (Session["id"] != null && Session["name"].ToString() == "employee")
            {
                id = Convert.ToInt16(Session["id"]);

                var employeeSalary = db.Salaries.Where(x => x.EmployeeId == id).OrderByDescending(x => x.SalaryId).ToList();

                return View(employeeSalary);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}