using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class OvertimePay
    {
        public double OvertimeHours { get; set; }
        public double OvertimeRate { get; set; }
        public int PaystubID { get; set; }
    }
}