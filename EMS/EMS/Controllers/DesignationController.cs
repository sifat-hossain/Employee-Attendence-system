using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMS.Models;

namespace EMS.Controllers
{
    public class DesignationController : Controller
    {
        private EMSEntities db = new EMSEntities();

        // GET: Designation
        public ActionResult Index()
        {
            if (Session["name"].ToString() == "admin")
            {
                var designations = db.Designations.Include(d => d.Department);
                return View(designations.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Details(int? id)
        {
            if (Session["name"].ToString() == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Designation designation = db.Designations.Find(id);
                if (designation == null)
                {
                    return HttpNotFound();
                }
                return View(designation);
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
                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DesignationId,DepartmentId,DesignationName,SPH")] Designation designation)
        {
            if (ModelState.IsValid)
            {
                db.Designations.Add(designation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", designation.DepartmentId);
            return View(designation);
        }

        // GET: Designation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["name"].ToString() == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Designation designation = db.Designations.Find(id);
                if (designation == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", designation.DepartmentId);
                return View(designation);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Designation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DesignationId,DepartmentId,DesignationName,SPH")] Designation designation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(designation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", designation.DepartmentId);
            return View(designation);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
