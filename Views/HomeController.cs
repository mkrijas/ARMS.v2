
using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookieAuthenticationDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        
       
        [Authorize(Roles = "Admin")]
        public ActionResult AdminUser()
        {
            var uses = new UserModel();
            return View("Users", uses.IsAdmin);
        }
    }
}
