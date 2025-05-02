using Microsoft.AspNetCore.Mvc;
using MvcLightphrm_Prject.Models.Admin;
using MvcLightphrm_Prject.Models.Site;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace MvcLightphrm_Prject.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminRepo _adminRepo;

        public AdminController(IConfiguration config)
        {
            _adminRepo = new AdminRepo(config);
        }

        public IActionResult Adminlog()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Site", "Site"); // or your dashboard/home
            }

            return View();


        }


        //[HttpPost]
        //public IActionResult getLogin([FromForm] Admin data)
        //{
        //    var admins = _adminRepo.GetAdmins();

        //    var result = _adminRepo.GetSpecificAdmin(data);

        //    if (result == "True")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        TempData["SuccessMessage"] = result;

        //        return RedirectToAction("Adminlog");
        //    }

        //}


        [HttpPost]
        public async Task<IActionResult> getLogin([FromForm] Admin data)
        {
            var result = _adminRepo.GetSpecificAdmin(data);

            if (result == "True")
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, data.Email),
            new Claim(ClaimTypes.Role, "Admin") 
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction( "Index", "Home");
            }
            else
            {
                TempData["SuccessMessage"] = result;
                return RedirectToAction("Adminlog");
            }
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return Content("Hello Ramesh Vanaparthi");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAdmin()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Adminlog", "Admin");
        }

    }
}
