using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class Employee
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime DateEmployed { get; set; }
        public bool isManager { get; set; }
    }
}