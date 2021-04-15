using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class Pay
    {
        public double GrossPay { get; set; }
        public double HoursLogged { get; set; }
        public double HourlyRate { get; set; }
        public int PaystubID { get; set; }
    }
}