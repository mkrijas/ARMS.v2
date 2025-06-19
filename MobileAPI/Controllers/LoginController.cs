using ARMS.JwtHelpers;
using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Core.BaseModels.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<LoginController> _logger;
        private readonly JwtSettings jwtSettings;
        private readonly IUserService _userService;
        private readonly IDbService _dbService;
        private readonly IMobileNotificationService _mobileNotificationService;

        public LoginController(JwtSettings jwtSettings,

            ILogger<LoginController> logger,
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            IUserService userService,
            IDbService dbService,
            IMobileNotificationService mobileNotificationService
            )
        {
            this.jwtSettings = jwtSettings;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userService = userService;
            _dbService = dbService;
            _mobileNotificationService = mobileNotificationService;
        }

        [HttpPost]
        public async Task<ActionResult> Login_Approval(string UserName, string Password, string DeviceID)
        {
            
            UserModel user = new UserModel() { UserID = UserName, PasswordHash = Password, DeviceID = DeviceID };
            var result = await _signInManager.PasswordSignInAsync(user.UserID, user.PasswordHash, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if(UserName.ToUpper() == "ADMIN")
                {
                    _dbService.ChangeConnectionString("ArmsDBTest");
                }
                else
                {
                    _dbService.ChangeConnectionString("ArmsDB");
                }
                var currentConnection = _dbService.GetCurrentConnectionString();
                string Operation = "APPROVE";
                int? recordStatus = _userService.SelectDeviceExists(UserName, DeviceID, Operation);
                if (recordStatus.HasValue)
                {
                    if (recordStatus.Value == -1 || recordStatus.Value == 0)
                    {
                        _userService.UpdateDeviceDetails(UserName, DeviceID);
                        return StatusCode(401, "Not allowed");
                    }
                    else if (recordStatus.Value == 1)
                    {
                        return StatusCode(401, "Not allowed");
                    }
                    else if (recordStatus.Value == 3)
                    {
                        _logger.LogInformation("User logged in.");
                        var Token = new UserTokens();
                        Token = JwtHelpers.GenTokenkey(new UserTokens()
                        {
                            EmailId = user.Email,
                            GuidId = Guid.NewGuid(),
                            UserName = UserName,
                            Id = Guid.NewGuid()//user.Id,

                        }, jwtSettings);
                        user.Token = Token.Token.ToString();

                        var response = new
                        {
                            Status = "Success",
                            Token = user.Token
                        };

                        return StatusCode(200, response);
                        
                    }
                }
                else
                {
                    _userService.UpdateDeviceDetails(UserName, DeviceID);
                }

                //return StatusCode(201, "Success"), user.Token;
                //return Task.FromResult(StatusCode(201, "Success"));

                ////return LocalRedirect(returnUrl);
            }
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("NotConfirmed", "Contact admin for approval");
                return StatusCode(401, "Not allowed");
                //return Page();
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //return Page();
                return StatusCode(501, "Invalid");
            }
            //return (user);
            //return StatusCode(201, "Success");
        }

        // New endpoint to return the current connection string
        [HttpGet("GetCurrentConnectionString")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<string> GetCurrentConnectionString()
        {
            var currentConnectionString = _dbService.GetCurrentConnectionString();
            if (string.IsNullOrEmpty(currentConnectionString))
            {
                return NotFound("Connection string not found.");
            }
            return Ok(currentConnectionString);
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> UpdateMobileNotification(MobileNotificationModel updateModel)
        {
            string result = "";
            var returnModel = _mobileNotificationService.UpdateMobileNotification(updateModel);
            if (returnModel != null)
            {
                result = "Updated Successfully.";
            }
            return result;
        }
    }
}
