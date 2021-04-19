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
                { new Paystub { EmployeeUsername = "nmd0005", StartDate = new DateTime(2021, 4, 11, 8, 0, 0), EndDate = new DateTime(2021, 4, 18, 8, 0, 0), PayDeductionsID = 1, PaystubID = 1 } },
                { new Paystub { EmployeeUsername = "nmd0005", StartDate = new DateTime(2021, 4, 3, 8, 0, 0), EndDate = new DateTime(2021, 4, 10, 8, 0, 0), PayDeductionsID = 1, PaystubID = 2 } },
                { new Paystub { EmployeeUsername = "nmd0005", StartDate = new DateTime(2021, 3, 26, 8, 0, 0), EndDate = new DateTime(2021, 4, 2, 8, 0, 0), PayDeductionsID = 1, PaystubID = 3 } },
                { new Paystub { EmployeeUsername = "nmd0005", StartDate = new DateTime(2021, 3, 18, 8, 0, 0), EndDate = new DateTime(2021, 3, 25, 8, 0, 0), PayDeductionsID = 1, PaystubID = 4 } }
            },
            Pays = new List<Pay>
            {
                { new Pay { GrossPay = 1000, PaystubID = 1, HourlyRate = 25, HoursLogged = 40 } },
                { new Pay { GrossPay = 1000, PaystubID = 2, HourlyRate = 25, HoursLogged = 40 } },
                { new Pay { GrossPay = 1000, PaystubID = 3, HourlyRate = 25, HoursLogged = 40 } },
                { new Pay { GrossPay = 1000, PaystubID = 4, HourlyRate = 25, HoursLogged = 40 } }
            },
            BonusPays = new List<BonusPay>
            {
                {new BonusPay { PaystubID = 1, BonusAmount = 500 } },
                {new BonusPay { PaystubID = 4, BonusAmount = 250 } }
            },
            CharitableDonations = new List<CharitableDonation>
            {
                { new CharitableDonation { PayDeductionsID = 1, DonationAmount = 200, EndDate = new DateTime(2021, 4, 11, 8, 0, 0), Recipient = "St. Jude's" } }
            },
            OvertimePays = new List<OvertimePay>
            {
                { new OvertimePay { PaystubID = 2, OvertimeHours = 10, OvertimeRate = 37.5 } }
            },
            PayDeductions = new List<PayDeductions>
            {
                { new PayDeductions { PayDeductionsID = 1, EmployeeUsername = "nmd0005" } }
            },
            RetirementContributions = new List<RetirementContribution> {
                { new RetirementContribution { PayDeductionsID = 1, ContributionAmount = 100, EndDate = new DateTime(2021, 4, 18, 8, 0, 0) } }
            },
            TaxDeductions = new List<TaxDeduction>
            {
                { new TaxDeduction { PayDeductionsID = 1, FederalTax = 50, StateTax = 20, Medicare = 15.75, SocialSecurity = 5.5 } }
            },
            WageGarnishments = new List<WageGarnishment>
            {
                { new WageGarnishment { PayDeductionsID = 2, EndDate = new DateTime(2021, 4, 18, 8, 0, 0), Amount = 125, Description = "Child Support!" } }
            }
        };

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

        public static bool isManager()
        {
            if (System.Web.HttpContext.Current.Session["isManager"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
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
                return RedirectToAction("ViewPaystubs");
            }
            else
            {
                if (db.Employees.FirstOrDefault(e => e.Username.ToLower() == login.Username.ToLower()) != null)
                {
                    ModelState.AddModelError("Password", "Incorrect password. Please try again.");
                    return View(login);
                }
                else
                {
                    ModelState.AddModelError("Username", "No employees by that username were found.");
                    return View(login);
                }

            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
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
            var retirement = db.RetirementContributions.FirstOrDefault(r => r.PayDeductionsID == deductions.PayDeductionsID && r.EndDate < paystub.StartDate);
            var taxes = db.TaxDeductions.FirstOrDefault(t => t.PayDeductionsID == deductions.PayDeductionsID);
            var donations = db.CharitableDonations.Where(d => d.PayDeductionsID == deductions.PayDeductionsID && d.EndDate < paystub.StartDate).ToList();
            var garnish = db.WageGarnishments.Where(g => g.PayDeductionsID == deductions.PayDeductionsID && g.EndDate < paystub.StartDate).ToList();
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

            var overtimePay = (overtime?.OvertimeHours ?? 0) * (overtime?.OvertimeRate ?? 0);
            var totalPay = overtimePay + totalBonus + pay.GrossPay;
            var payAfter = totalPay - totalGarnishment - totalDonations - taxes.StateTax - taxes.FederalTax - taxes.SocialSecurity - taxes.Medicare - (retirement?.ContributionAmount ?? 0);

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