using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcLightphrm_Prject.Models.Site;

namespace MvcLightphrm_Prject.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class siteApiController : ControllerBase
    {
        private readonly SiteRepo _siteRepo;

        public siteApiController(SiteRepo siteRepo)
        {
            _siteRepo = siteRepo;
        }





    }
}
