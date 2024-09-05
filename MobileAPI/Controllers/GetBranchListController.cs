using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetBranchListController : ControllerBase
    {
        private readonly IUserService _userService;
        public GetBranchListController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<UserBranchRoleModel> GetBranchList(string UserID)
        {
            return _userService.GetBranchesNRoles(UserID);
        }
    }
}
