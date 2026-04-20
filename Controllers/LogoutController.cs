using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ArmsModels.BaseModels;
using System.Threading.Tasks;

namespace Views.Controllers
{
    [Route("UserAccount/[controller]")]
    public class LogoutController : Controller
    {
        private readonly SignInManager<UserModel> _signInManager;

        public LogoutController(SignInManager<UserModel> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
