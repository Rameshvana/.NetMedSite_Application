using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcLightphrm_Prject.Models.Product;
using MvcLightphrm_Prject.Models.Site;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MvcLightphrm_Prject.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductRepo _productRepo;

        private readonly SiteRepo _siteRepo;

        public ProductController(ProductRepo productRepo, SiteRepo siteRepo)
        {
            _productRepo = productRepo;
            _siteRepo = siteRepo;
        }



        public IActionResult Product(string searchTerm)
        {
            var products_list = _productRepo.GetProducts();
            Debug.WriteLine(products_list);

            if (!String.IsNullOrEmpty(searchTerm))
            {
                products_list = products_list
                .Where(s => s.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }

            return View(products_list);
        }

        public IActionResult AddProduct()
        {
            var sites = _siteRepo.GetSites();

            //var siteList = sites.Select(s => new SelectListItem
            //{
            //    Value = s.SiteId.ToString(),
            //    Text = s.SiteName
            //}).ToList();

            //var model = new Product
            //{
            //    SiteList = siteList
            //};

            ViewBag.sites = sites;

            return View();
        }


        [HttpPost]
        public IActionResult SaveProduct([FromForm] Product data)
        {
            

            if (ModelState.IsValid)
            {
                var resultMessage = _productRepo.InsertProduct(data);
                TempData["SuccessMessage"] = resultMessage;

                return RedirectToAction("Product");
            }

            //var sites = _siteRepo.GetSites();
            //data.SiteList = sites.Select(s => new SelectListItem
            //{
            //    Value = s.SiteId.ToString(),
            //    Text = s.SiteName
            //}).ToList();

            return View("AddProduct", data);
        
        }

        [HttpPost]
        public IActionResult Delete(int ProductId)
        {
            Debug.WriteLine(ProductId);

            var message = _productRepo.DeleteProduct(ProductId);

            if (message != null)
            {
                TempData["SuccessMessage"] = message;

                return RedirectToAction("Product");
            }

            return RedirectToAction("Product");
        }

        public IActionResult Update(int ProductId)
        {
            var product = _productRepo.GetProducts().FirstOrDefault(p => p.ProductId == ProductId);

            if (product == null)
                return NotFound();

            // Get list of sites for dropdown
            var sites = _siteRepo.GetSites();

            //product.SiteList = sites.Select(s => new SelectListItem
            //{
            //    Value = s.SiteName,
            //    Text = s.SiteName
            //}).ToList();

            ViewBag.sites = sites;

            return View(product);

        }

        [HttpPost]
        public IActionResult UpdateProduct([FromForm] Product data)
        {
            var sites = _siteRepo.GetSites();

            if (ModelState.IsValid)
            {

                //var site = sites.FirstOrDefault(s => s.SiteName == data.SiteName);

                //if (site != null)
                //    data.SiteId = site.SiteId;

                var message = _productRepo.UpdateProduct(data);

                TempData["SuccessMessage"] = message;
                return RedirectToAction("Product");

            }


            //data.SiteList = sites.Select(s => new SelectListItem
            //{
            //    Value = s.SiteName,
            //    Text = s.SiteName
            //}).ToList();

            return RedirectToAction("Update", data);
        }

    }
}
