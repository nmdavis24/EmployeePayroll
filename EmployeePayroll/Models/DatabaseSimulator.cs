using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models
{
    public class DatabaseSimulator
    {
        public ICollection<BonusPay> BonusPays { get; set; }
        public ICollection<CharitableDonation> CharitableDonations { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<OvertimePay> OvertimePays { get; set; }
        public ICollection<Pay> Pays { get; set; }
        public ICollection<PayDeductions> PayDeductions { get; set; }
        public ICollection<Paystub> Paystubs { get; set; }
        public ICollection<RetirementContribution> RetirementContributions { get; set; }
        public ICollection<TaxDeduction> TaxDeductions { get; set; }
        public ICollection<WageGarnishment> WageGarnishments { get; set; }
    }
}