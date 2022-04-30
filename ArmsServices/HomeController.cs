using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ArmsModels.BaseModels;

namespace ArmsServices
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        [Authorize(Roles = "IsAdmin")]
        public ActionResult AdminUser()
        {
            var uses = new UserModel();
            return View("Users", uses.IsAdmin);
        }
    }
}
