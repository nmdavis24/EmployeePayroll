using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePayroll.Models;

namespace EmployeePayroll.Controllers
{
    public class EmployeeController : Controller
    {
        public static DatabaseSimulator db = new DatabaseSimulator();

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            // finds employee row in database with matching username and password. null if none found
            var person = db.Employees.FirstOrDefault(e => e.Username == username && e.Password == password);
            if (person != null)
            {
                // session info used to check if users are logged in throughout the application
                Session["Username"] = person.Username;
                if (person.isManager)
                {
                    Session["isManager"] = "true";
                }
                else
                {
                    Session["isManager"] = "false";
                }
                return Json(new
                {
                    success = true
                });
            }
            else
            {
                if (db.Employees.FirstOrDefault(e => e.Username == username) != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Incorrect password. Please try again."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "No employees by that username were found."
                    });
                }

            }
        }

        public ActionResult ViewPaystubs()
        {
            string username = Session["Username"].ToString();
            Employee emp = db.Employees.FirstOrDefault(e => e.Username == username);
            //List<string> paystubDates = new List<string>();
            List<Paystub> paystubs = db.Paystubs.Where(e => e.EmployeeUsername == username).ToList();
            // get dates to populate paystub selection dropdown
            //foreach (var stub in paystubs.OrderByDescending(p => p.StartDate))
            //{
            //    paystubDates.Add(stub.StartDate + " - " + stub.EndDate);
            //}
            return View(paystubs);
        }

        public PartialViewResult PaystubInfo(int paystubID)
        {

        }
    }
}