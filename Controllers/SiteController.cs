using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcLightphrm_Prject.Models.Site;
using System.Diagnostics;

namespace MvcLightphrm_Prject.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SiteController : Controller
    {
        private readonly SiteRepo _repo;

        public SiteController(IConfiguration config)
        {
            _repo = new SiteRepo(config);
        }
        public IActionResult Site(string searchTerm)
        {
            var sites = new List<Site>
        {
            new Site
            {
                SiteId = 1,
                SiteName = "Site A",
                Description = "Description for Site A",
                Address = "123 Main St, City A",
                SiteCode = "A001",
                SiteType = "Type1"
            },
            new Site
            {
                SiteId = 2,
                SiteName = "Site B",
                Description = "Description for Site B",
                Address = "456 Elm St, City B",
                SiteCode = "B002",
                SiteType = "Type2"
            },
            new Site
            {
                SiteId = 3,
                SiteName = "Site C",
                Description = "Description for Site C",
                Address = "789 Oak St, City C",
                SiteCode = "C003",
                SiteType = "Type1"
            },
            new Site
            {
                SiteId = 4,
                SiteName = "Site D",
                Description = "Description for Site D",
                Address = "321 Pine St, City D",
                SiteCode = "D004",
                SiteType = "Type3"
            },
            new Site
            {
                SiteId = 5,
                SiteName = "Site E",
                Description = "Description for Site E",
                Address = "654 Maple St, City E",
                SiteCode = "E005",
                SiteType = "Type2"
            }
        };

            var a = new List<Site>();
            Debug.WriteLine(searchTerm);

            var sites_List = _repo.GetSites();
            Debug.WriteLine(a);

            if (!String.IsNullOrEmpty(searchTerm))
            {
                sites_List = sites_List
                .Where(s => s.SiteName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }

            return View(sites_List);
        }

        public IActionResult AddSite()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveSite([FromForm] Site data)
        {
            if (ModelState.IsValid)
            {
                var resultMessage = _repo.InsertSite(data);

                TempData["SuccessMessage"] = resultMessage;

                return RedirectToAction("Site");
            }

            return RedirectToAction("AddSite", data);
        }

        [HttpPost]
        public IActionResult Delete(int siteId)
        {
            Debug.WriteLine(siteId);

            var message = _repo.DeleteSite(siteId);

            if (message != null)
            {
                TempData["SuccessMessage"] = message;

                return RedirectToAction("Site");
            }

            return RedirectToAction("Site");
        }


        public IActionResult Update(int SiteId)
        {
            var sites_List = _repo.GetSites();

            var site = sites_List.FirstOrDefault(s => s.SiteId == SiteId);

            return View(site);
        }

        [HttpPost]
        public IActionResult UpdateSite([FromForm] Site data)
        {
            Debug.WriteLine(data);
            string message = _repo.UpdateSite(data);

            if (message != null)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Site");
            }

            return View("Update", data);
        }
    }
}
