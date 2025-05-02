using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcLightphrm_Prject.Models.Product;
using MvcLightphrm_Prject.Models.Site;
using MvcLightphrm_Prject.Models.User;
using System.Diagnostics;

namespace MvcLightphrm_Prject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ProductRepo _productRepo;

        private readonly SiteRepo _siteRepo;

        private readonly UserRepo _userRepo;


        public UserController(ProductRepo productRepo, SiteRepo siteRepo, UserRepo userRepo )
        {
            _productRepo = productRepo;
            _siteRepo = siteRepo;
            _userRepo = userRepo;
        }

   

        public IActionResult User(string searchTerm)
        {
            var users_list = _userRepo.GetUsers();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                users_list = users_list
                .Where(s => s.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }

            return View(users_list);

        }

        public IActionResult AddUser()
        {
            var sites = _siteRepo.GetSites();
            var products = _productRepo.GetProducts();

            ViewBag.sites = sites;
            ViewBag.products = products;
            return View();
        }

        [HttpPost]
        public IActionResult SaveUser([FromForm] User data)
        {
            var users_list = _userRepo.GetUsers();

            //bool isDuplicate = users_list.Any(p => p.UserName == data.UserName);

            //if (isDuplicate)
            //{
            //    // Add a custom error to ModelState for the Name field
            //    ModelState.AddModelError("UserName", "Name is already taken.");
            //    //return RedirectToAction("AddUser", data);
            //    return View(data);
            //}
            if (ModelState.IsValid)
            {
                var resultMessage = _userRepo.InsertUser(data);

                TempData["SuccessMessage"] = resultMessage;

                return RedirectToAction("User");
            }


            return View("AddUser", data);
        }

        public IActionResult Delete(int userId)
        {
            Debug.WriteLine(userId);

            var message = _userRepo.DeleteUser(userId);

            if (message != null)
            {
                TempData["SuccessMessage"] = message;

                return RedirectToAction("User");
            }

            return RedirectToAction("User");
        }

        [HttpPost]
        public IActionResult Update(int userId)
        {
            var sites = _siteRepo.GetSites(); 
            var user = _userRepo.GetSpecificUser(userId); 

            var siteProducts = _productRepo.GetProductsBySiteId(user.SiteId);

            ViewBag.Sites = sites;
            ViewBag.Products = siteProducts;

            return View(user);
        }


        [HttpPost]
        public IActionResult UpdateUser([FromForm] User data)
        {
            var sites = _siteRepo.GetSites();

            if (ModelState.IsValid)
            {

                var message = _userRepo.UpdateUser(data);

                TempData["SuccessMessage"] = message;
                return RedirectToAction("User");

            }


            //data.SiteList = sites.Select(s => new SelectListItem
            //{
            //    Value = s.SiteName,
            //    Text = s.SiteName
            //}).ToList();

            return RedirectToAction("Update", data);
        }

        [HttpPost]
        public JsonResult GetProductsBySiteId(int siteId)
        {
            var products = _productRepo.GetProductsBySiteId(siteId); 
            return Json(products);
        }

    }
}
