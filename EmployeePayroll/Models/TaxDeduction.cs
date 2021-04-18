using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class TaxDeduction
    {
        public double FederalTax { get; set; }
        public double StateTax { get; set; }
        public double SocialSecurity { get; set; }
        public double Medicare { get; set; }
        public int PayDeductionsID { get; set; }
    }
}