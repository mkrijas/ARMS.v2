using Core.BaseModels.User;
using Core.IDataServices.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class DeviceApprovalController : ControllerBase
    {
        private readonly IDeviceService IDeviceService;
        public DeviceApprovalController(IDeviceService _IDeviceService)
        {
            IDeviceService = _IDeviceService;
        }

        [HttpGet("[action]/")]
        public async Task<IEnumerable<DeviceModel>> Select(int? NoOfRecords, int? BranchId)
        {
            return IDeviceService.Select("PENDING").ToList();
        }

        [HttpPost("[action]/")]
        public async Task<string> Approve(DeviceModel model)
        {
            string result = "";
           
            var returnModel = IDeviceService.Approve(model);
            if (returnModel != null)
            {
                result = "Approved Successfully.";
            }
            
            return result;
        }

        [HttpPost("[action]/")]
        public async Task<string> Deny(DeviceModel model)
        {
            string result = "";
            var returnModel = IDeviceService.Deny(model);
            if (returnModel != null)
            {
                result = "Denied Successfully.";
            }
           
            return result;
        }

    }
}