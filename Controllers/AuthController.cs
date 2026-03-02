using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ArmsModels.BaseModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ArmsServices.DataServices;

namespace Views.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<RoleModel> _roleManager;

        public AuthController(UserManager<UserModel> userManager, IUserService userService, RoleManager<RoleModel> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        public class SignInDto { public string UserID { get; set; } }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
        {
            if (string.IsNullOrEmpty(dto?.UserID)) return BadRequest();

            // Validate user exists
            var appUser = await _userManager.FindByIdAsync(dto.UserID);
            if (appUser == null) return BadRequest();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, dto.UserID) };

            // Attach branch/role claims using server-side user service
            var currentRole = _userService.GetCurrentBranchRole(dto.UserID);
            if (currentRole != null && currentRole.Branch != null)
            {
                claims.Add(new Claim("BranchID", currentRole.Branch.BranchID.ToString()));
                if (!string.IsNullOrEmpty(currentRole.Branch.BranchName))
                    claims.Add(new Claim("BranchName", currentRole.Branch.BranchName));
            }

            if (currentRole?.Role != null && !string.IsNullOrEmpty(currentRole.Role.RoleID))
            {
                claims.Add(new Claim(ClaimTypes.Role, currentRole.Role.RoleID));
                var claimsFromRole = await _roleManager.GetClaimsAsync(currentRole.Role);
                foreach (var c in claimsFromRole) claims.Add(c);
            }

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = System.DateTimeOffset.UtcNow.AddDays(30)
            });

            return Ok();
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok();
        }
    }
}
