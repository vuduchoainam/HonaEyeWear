using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HonaEyeWear.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin")]
    [Authorize]
    public class ManageAdminController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
