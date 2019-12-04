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
    public class AttendenceController : Controller
    {
        private EMSEntities db = new EMSEntities();

        public ActionResult Index()
        {
            if (Session["name"].ToString() == "admin")
            {
                return View(db.Attendences.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string email, string password)
        {
            DateTime date = DateTime.Now;

            Attendence attendence = new Attendence();
            var value = db.EmployeeInfoes.Where(x => x.EmployeeEmail == email && x.EmployeePassword == password).FirstOrDefault();
            if(value!=null)
            {
              var atd = db.Attendences.ToList().Exists(x=> x.EmployeeId == value.EmployeeId && x.Date == date.Date && x.EntryTime!=null );
          
                if(atd==false)
                {
                    attendence.EmployeeId = value.EmployeeId;
                    attendence.EntryTime = DateTime.Now.TimeOfDay;
                    attendence.Date = DateTime.Now;

                    db.Attendences.Add(attendence);
                    db.SaveChanges();

                    return Content("<script language='javascript' type='text/javascript'>alert('Successfully Login'); window.location='/Attendence/Create'; </script>");

                }
                else
                {
                    var data = db.Attendences.Where(x => x.EmployeeId == value.EmployeeId && x.Date == date.Date).FirstOrDefault();
                    data.EmployeeId = value.EmployeeId;
                    data.OutTime = DateTime.Now.TimeOfDay;
                    data.Date = DateTime.Now;

                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();

                    return Content("<script language='javascript' type='text/javascript'>alert('Successfully Logout'); window.location='/Attendence/Create'; </script>");
                }
            }
            return View();
        }

        public ActionResult AttendenceById(int? id)
        {
            if (Session["id"] != null && Session["name"].ToString() == "employee")
            {

                id = Convert.ToInt32(Session["id"]);
                ViewBag.Name = db.EmployeeInfoes.Where(x => x.EmployeeId == id).FirstOrDefault().EmployeeName;
                var value = db.Attendences.Where(x => x.EmployeeId == id).ToList();
                return View(value);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}