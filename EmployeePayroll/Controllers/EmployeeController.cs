using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePayroll.Models;
using EmployeePayroll.Models.ViewModels;

namespace EmployeePayroll.Controllers
{
    public class EmployeeController : Controller
    {
        public static DatabaseSimulator db = new DatabaseSimulator
        {
            Employees = new List<Employee>
            {
                { new Employee { Username = "nmd0005", FirstName = "Noah", LastName = "Davis", Password = "HelloWorld123", Address = "1957 Wire Road Auburn, AL 36832", DateEmployed = new DateTime(2020, 4, 18, 8, 0, 0), isManager = true } },
                { new Employee { Username = "mmb0081", FirstName = "Miller", LastName = "Barnes", Password = "HelloThere123", Address = "123 Main Street Auburn, AL 36830", DateEmployed = new DateTime(2020, 4, 18, 8, 0, 0), isManager = false }}
            },
            Paystubs = new List<Paystub>
            {

            },
            Pays = new List<Pay>
            {

            },
            BonusPays = new List<BonusPay>
            {

            },
            CharitableDonations = new List<CharitableDonation>
            {

            },
            OvertimePays = new List<OvertimePay>
            {

            },
            PayDeductions = new List<PayDeductions>
            {

            },
            RetirementContributions = new List<RetirementContribution> {
            
            },
            TaxDeductions = new List<TaxDeduction>
            {

            },
            WageGarnishments = new List<WageGarnishment>
            {

            }
        };

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public static bool isLoggedIn()
        {
            if (System.Web.HttpContext.Current.Session["Username"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(LoginViewModel login)
        {
            // finds employee row in database with matching username and password. null if none found
            var person = db.Employees.FirstOrDefault(e => e.Username.ToLower() == login.Username.ToLower() && e.Password == login.Password);
            if (person != null)
            {
                // session info used to check if users are logged in throughout the application
                Session["Username"] = person.Username;
                if (person.isManager)
                {
                    Session["isManager"] = "true";
                }
                else
                {
                    Session["isManager"] = "false";
                }
                return Json(new
                {
                    success = true
                });
            }
            else
            {
                if (db.Employees.FirstOrDefault(e => e.Username.ToLower() == login.Username.ToLower()) != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Incorrect password. Please try again."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "No employees by that username were found."
                    });
                }

            }
        }

        public ActionResult ViewPaystubs()
        {
            if (isLoggedIn())
            {
                string username = Session["Username"].ToString();
                Employee emp = db.Employees.FirstOrDefault(e => e.Username == username);
                List<Paystub> paystubs = db.Paystubs.Where(e => e.EmployeeUsername == username).ToList();
                return View(paystubs);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public PartialViewResult _PaystubInfo(int paystubID)
        {
            // get all the data we need from db
            // FirstOrDefault implies one-to-one relationship
            // Where implies one-to-many

            // pay
            var paystub = db.Paystubs.FirstOrDefault(p => p.PaystubID == paystubID);
            var pay = db.Pays.FirstOrDefault(p => p.PaystubID == paystubID);
            var bonuses = db.BonusPays.Where(b => b.PaystubID == paystubID).ToList();
            var overtime = db.OvertimePays.FirstOrDefault(d => d.PaystubID == paystubID);
            // deductions
            var deductions = db.PayDeductions.FirstOrDefault(p => p.EmployeeUsername == paystub.EmployeeUsername);
            var retirement = db.RetirementContributions.FirstOrDefault(r => r.PayDeductionsID == deductions.PayDeductionsID);
            var taxes = db.TaxDeductions.FirstOrDefault(t => t.PayDeductionsID == deductions.PayDeductionsID);
            var donations = db.CharitableDonations.Where(d => d.PayDeductionsID == deductions.PayDeductionsID).ToList();
            var garnish = db.WageGarnishments.Where(g => g.PayDeductionsID == deductions.PayDeductionsID).ToList();
            // employee info
            var employee = db.Employees.FirstOrDefault(e => e.Username == paystub.EmployeeUsername);

            double totalBonus = 0;
            foreach (var bonus in bonuses)
            {
                // add up bonuses
                totalBonus += bonus.BonusAmount;
            }

            double totalGarnishment = 0;
            foreach (var gar in garnish)
            {
                totalGarnishment += gar.Amount;
            }

            double totalDonations = 0;
            foreach (var don in donations)
            {
                totalDonations += don.DonationAmount;
            }

            var overtimePay = overtime.OvertimeHours * overtime.OvertimeRate;
            var totalPay = overtimePay + totalBonus + pay.GrossPay;
            var payAfter = totalPay - totalGarnishment - totalDonations - taxes.StateTax - taxes.FederalTax - taxes.SocialSecurity - taxes.Medicare - retirement.ContributionAmount;

            // get all the data we need and put it in the viewmodel
            PaystubViewModel vm = new PaystubViewModel
            {
                BonusPay = totalBonus,
                Donations = donations,
                OvertimeHours = overtime?.OvertimeHours ?? 0,
                OvertimeRate = overtime?.OvertimeRate ?? 0,
                OvertimePay = overtimePay,
                HoursLogged = pay.HoursLogged,
                HourlyRate = pay.HourlyRate,
                NormalPay = pay.GrossPay,
                TotalPay = totalPay,
                PayAfterDeductions = payAfter,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Dates = "" + paystub.StartDate + " - " + paystub.EndDate,
                StateTax = taxes.StateTax,
                FederalTax = taxes.FederalTax,
                SocialSecurity = taxes.SocialSecurity,
                Medicare = taxes.Medicare,
                // if retirement is null make it 0
                RetirementContribution = retirement?.ContributionAmount ?? 0,
                WageGarnishment = totalGarnishment
            };

            return PartialView(vm);
        }
    }
}