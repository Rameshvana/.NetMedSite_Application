using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcLightphrm_Prject.Models.Product
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductFullName { get; set; }

        [ValidateNever]
        public string UserId { get; set; }
        public int SiteId { get; set; }
        [ValidateNever]
        public string SiteName { get; set; }

        public DateTime ApprovalDate { get; set; }


        [ValidateNever]
        public List<SelectListItem> SiteList { get; set; }

        public Product() { }
    }
}
