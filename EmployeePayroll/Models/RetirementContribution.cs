using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class RetirementContribution
    {
        public double ContributionAmount { get; set; }
        public DateTime EndDate { get; set; }
        public int PayDeductionsID { get; set; }
    }
}