using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePayroll.Models.ViewModels
{
    public class PaystubViewModel
    {
        public double BonusPay { get; set; }
        public List<CharitableDonation> Donations { get; set; }
        public double OvertimeHours { get; set; }
        public double OvertimeRate { get; set; }
        public double HoursLogged { get; set; }
        public double HourlyRate { get; set; }
        public double NormalPay { get; set; }
        public double OvertimePay { get; set; }
        public double TotalPay { get; set; }
        public double PayAfterDeductions { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dates { get; set; }
        public double RetirementContribution { get; set; }
        public double FederalTax { get; set; }
        public double StateTax { get; set; }
        public double SocialSecurity { get; set; }
        public double Medicare { get; set; }
        public double WageGarnishment { get; set; }
    }
}