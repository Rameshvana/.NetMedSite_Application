using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcLightphrm_Prject.Models.User
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int SiteId { get; set; } // Foreign Key
        [ValidateNever]
        public string SiteName { get; set; }
        public int ProductId { get; set; } // Foreign Key
        [ValidateNever]
        public string ProductName { get; set; }
        [ValidateNever]
        //public string SiteName { get; set; }

        public List<SelectListItem> SiteList { get; set; }


    }
}

