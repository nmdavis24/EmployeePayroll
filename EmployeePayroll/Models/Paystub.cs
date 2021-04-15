using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class Paystub
    {
        public int PaystubID { get; set; }
        public string EmployeeUsername { get; set; }
        public int PayDeductionsID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}