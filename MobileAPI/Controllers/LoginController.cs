using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<LoginController> _logger;
        private readonly JwtSettings jwtSettings;

        public LoginController(JwtSettings jwtSettings,
            
            ILogger<LoginController> logger,
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager
            )
        {
            this.jwtSettings = jwtSettings;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<UserModel> Login_Approval(string UserName, string Password, string DeviceID)
        {
            UserModel user = new UserModel() { UserID = UserName, PasswordHash = Password, DeviceID = DeviceID};
            var result = await _signInManager.PasswordSignInAsync(user.UserID, user.PasswordHash, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                //return LocalRedirect(returnUrl);
            }
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("NotConfirmed", "Contact admin for approval");
                //return Page();
            }
            
            else
            {

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //return Page();
            }
            return (user);
        }
    }
}
