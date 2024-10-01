using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ArmsModels.BaseModels;


namespace Views.Pages.UserAccount
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private IDbService dbService;

        public LoginModel(SignInManager<UserModel> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<UserModel> userManager,
            IDbService _dbService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            dbService = _dbService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(20,ErrorMessage ="The {0} must be between {1} and {2} charecters long",MinimumLength = 3)]
            [Display(Name = "UserID")]
            public string UserID { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<ActionResult> OnGetAsync(string returnUrl = "~/UserAccount/Login")
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl ??= Url.Content("~/");
            
            var IsSignedIn =  _signInManager.IsSignedIn(HttpContext.User);
            if (IsSignedIn)
            {
                
                returnUrl = Url.Content("/");
                return LocalRedirect(returnUrl);
            };

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserID, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {                    
                    _logger.LogInformation("User logged in.");                               
                    return LocalRedirect(returnUrl);
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("NotConfirmed", "Contact admin for approval");
                    return Page();
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
