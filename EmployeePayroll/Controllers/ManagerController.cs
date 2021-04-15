using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePayroll.Models;

namespace EmployeePayroll.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddEmployee(string username, string first, string last, string password, string address)
        {
            try
            {
                Employee newEmp = new Employee
                {
                    Username = username,
                    FirstName = first,
                    LastName = last,
                    Password = password,
                    Address = address,
                    DateEmployed = DateTime.Now,
                    isManager = false
                };
                EmployeeController.db.Employees.Append(newEmp);
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong :("
                });
            }
        }

        public ActionResult DeleteEmployee(string username)
        {
            Employee emp = EmployeeController.db.Employees.FirstOrDefault(e => e.Username == username);
            EmployeeController.db.Employees.Remove(emp);
            return Json(new
            {
                success = true
            });
        }
    }
}