using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EMS.Models;

namespace EMS.Controllers
{
    public class HomeController : Controller
    {
        private EMSEntities db = new EMSEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            bool employee = db.EmployeeInfoes.ToList().Exists(x => x.EmployeeEmail == email && x.EmployeePassword == password);
            if (email == "admin" && password == "12345")
            {
                Session["id"] = "admin";
                Session["name"] = "admin";
                return RedirectToAction("Index", "Employee");
            }
            if (employee == true)
            {               
                    var employeeid = db.EmployeeInfoes.Where(x => x.EmployeeEmail == email && x.EmployeePassword == password).FirstOrDefault().EmployeeId;
                    Session["id"] = employeeid;
                    Session["name"] = "employee";
                    return RedirectToAction("EmployeeProfile", "Employee");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Your Email or Password is incorrect'); window.location='/Home/Login/'</script>");
            }

        }
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Abandon();
            Response.Cookies.Clear();
            Session["id"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}