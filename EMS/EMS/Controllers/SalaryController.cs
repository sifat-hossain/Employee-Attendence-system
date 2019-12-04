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
    public class SalaryController : Controller
    {
        private EMSEntities db = new EMSEntities();

        public ActionResult Index()
        {
            if (Session["name"].ToString() == "admin")
            {

                return View(db.Salaries.ToList());
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
                ViewBag.EmployeeId = new SelectList(db.EmployeeInfoes, "EmployeeId", "EmployeeName");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salary salary)
        {
            List<Attendence> atttendenceHour = null;

            int value = 0, hour = 0;
            var Todate = DateTime.Now.AddDays(-1);
            var data = db.Salaries.Where(x => x.EmployeeId == salary.EmployeeId).Count();
            if (data == 0)
            {
                atttendenceHour = db.Attendences.Where(x => x.EmployeeId == salary.EmployeeId).ToList();
            }
            else
            {
                var fromDate = db.Salaries.Where(x => x.EmployeeId == salary.EmployeeId).OrderByDescending(x => x.SalaryId).FirstOrDefault().Date;
                atttendenceHour = db.Attendences.Where(x => x.EmployeeId == salary.EmployeeId && x.Date >= fromDate && x.Date <= Todate).ToList();
            }
            foreach (var item in atttendenceHour)
            {
                int h = Convert.ToInt16(item.OutTime.Value.Hours - item.EntryTime.Value.Hours);
                hour += h;
            }
            var designation = db.EmployeeInfoes.Where(x => x.EmployeeId == salary.EmployeeId).FirstOrDefault().Designation.SPH;
            value = hour * (Convert.ToInt32(designation));
            int totalSalary = value + Convert.ToInt32(salary.Bonous);
            if (ModelState.IsValid)
            {
                salary.Allownce = value;
                salary.ToltalSalarey = totalSalary;
                salary.Date = DateTime.Now;
                db.Salaries.Add(salary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.EmployeeInfoes, "EmployeeId", "EmployeeName");
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
                Salary salary = db.Salaries.Find(id);
                if (salary == null)
                {
                    return HttpNotFound();
                }
                ViewBag.EmployeeId = new SelectList(db.EmployeeInfoes, "EmployeeId", "EmployeeName", salary.EmployeeId);
                return View(salary);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Salary salary)
        {
            salary.ToltalSalarey = salary.ToltalSalarey + salary.Bonous;
            salary.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(salary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.EmployeeInfoes, "EmployeeId", "EmployeeName", salary.EmployeeId);
            return View(salary);
        }

    }
}