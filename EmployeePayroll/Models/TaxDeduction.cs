using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class TaxDeduction
    {
        public double DeductionAmount { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public int PayDeductionsID { get; set; }
    }
}