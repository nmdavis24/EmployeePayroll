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

        public ActionResult EmployeeTable()
        {
            if (EmployeeController.isLoggedIn() && Session["isManager"] != null)
            {
                var employees = EmployeeController.db.Employees.ToList();
                return View(employees);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        /// <summary>
        /// Adds employee to simulated db
        /// </summary>
        /// <param name="username"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="password"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public ActionResult AddEmployee(string username, string first, string last, string password, string address, bool isManager)
        {
            try
            {
                var person = EmployeeController.db.Employees.FirstOrDefault(e => e.Username.ToLower() == username.ToLower());
                if (person != null)
                {
                    return Json(new
                    {
                        duplicate = true
                    });
                }
                
                Employee newEmp = new Employee
                {
                    Username = username,
                    FirstName = first,
                    LastName = last,
                    Password = password,
                    Address = address,
                    DateEmployed = DateTime.Now,
                    isManager = isManager
                };
                EmployeeController.db.Employees.Add(newEmp);
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
            try
            {
                Employee emp = EmployeeController.db.Employees.FirstOrDefault(e => e.Username == username);
                EmployeeController.db.Employees.Remove(emp);
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false
                });
            }
        }
    }
}