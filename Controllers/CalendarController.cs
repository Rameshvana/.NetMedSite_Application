using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcLightphrm_Prject.Models.Calendar;
using MvcLightphrm_Prject.Models.Product;
using MvcLightphrm_Prject.Models.Site;
using MvcLightphrm_Prject.Models.User;
using System.Diagnostics;
using CalendarModel = MvcLightphrm_Prject.Models.Calendar.Calendar;

namespace MvcLightphrm_Prject.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ProductRepo _productRepo;

        private readonly SiteRepo _siteRepo;
        private readonly UserRepo _userRepo;
        private readonly CalendarRepo _calRepo;

        public CalendarController(ProductRepo productRepo, SiteRepo siteRepo, UserRepo userRepo, CalendarRepo calRepo)
        {
            _productRepo = productRepo;
            _siteRepo = siteRepo;
            _userRepo = userRepo;
            _calRepo = calRepo;
        }

        public IActionResult Create()
        {
            List<int> years_list = new List<int>();
            for (int year = DateTime.Now.Year; year >= 2020; year--)
            {
                years_list.Add(year);
            }


            ViewBag.sites = _siteRepo.GetSites();

            ViewBag.users = _userRepo.GetUsers();

            return View();
        }



        public IActionResult SaveCalendar([FromForm] CalendarModel data)
        {
            var site = _siteRepo.GetSites().FirstOrDefault(s => s.SiteName == data.SiteName);

            if (site == null)
            {
                TempData["ErrorMessage"] = "Selected site not found.";
                return RedirectToAction("Create");
            }

            var unique = GenerateSiteUniqueId(site.SiteName);

            var SitesCalendar = _calRepo.GetAllCalendarSites();
            var siteCal = SitesCalendar.FirstOrDefault(s => s.SiteName == data.SiteName && s.Year == data.Year);

            if (siteCal != null)
            {
                TempData["ErrorMessage"] = "A calendar for this site and year already exists.";
                return RedirectToAction("Create");
            }

            var matchingProducts = _productRepo.GetProductsBySiteId(site.SiteId)
                .Where(pr => data.Year >= pr.ApprovalDate.Year)
                .ToList();

            if (!matchingProducts.Any())
            {
                TempData["ErrorMessage"] = "No products found for the selected site calendar year.";
                return RedirectToAction("Create");
            }

            var calendarSite = new CalendarModel
            {
                SiteName = data.SiteName,
                Year = data.Year,
                CalendarUniqueId = unique,
                UserName = data.UserName,
                ReportDate = DateTime.Today,
                SiteId = site.SiteId
            };

            string m1 = _calRepo.InsertSiteCalendar(calendarSite);

            foreach (var pr in matchingProducts)
            {
                var calendarProduct = new CalendarModel
                {
                    SiteName = data.SiteName,
                    Year = data.Year,
                    ProductName = pr.ProductName,
                    ApprovalDate = pr.ApprovalDate,
                    CalendarUniqueId = unique
                };

                string m2 = _calRepo.InsertProductSiteCalendar(calendarProduct);
            }

            return RedirectToAction("OpenCalendar");
        }




        public IActionResult ViewCalendar()
        {
            return View();
        }

        public IActionResult OpenCalendar()
        {
            var sitesCalendars = _calRepo.GetAllCalendarSites();
            return View(sitesCalendars);
        }
        [HttpPost]
        public IActionResult GetViewCalendar(string id)
        {
            Debug.WriteLine(id);
            var products = _calRepo.GetSpecficProductsSites(id);


            return View("ViewCalendar", products);
        }

        private string GenerateSiteUniqueId(string siteName)
        {
            string prefix = new string(siteName
                .Where(char.IsLetter)
                .Take(3)
                .ToArray())
                .ToUpper();

            string randomSuffix = Guid.NewGuid()
                .ToString("N")
                .Substring(0, 5)
                .ToUpper();

            return prefix + randomSuffix;
        }


    }

}
