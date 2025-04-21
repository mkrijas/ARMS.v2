using ArmsServices.DataServices;
using Core.BaseModels.Finance;
using Core.IDataServices.Finance.DayOpen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayOpenController : ControllerBase
    {
        private readonly IDayOpenService IDayOpenService;
        private readonly IUserService IUserService;
        public DayOpenController(IDayOpenService _IDayOpenService, IUserService _IUserService)
        {
            IDayOpenService = _IDayOpenService;
            IUserService = _IUserService;
        }

        public bool HasPermissionIDayOpenServiceApprove { get; set; } = false;
        public bool HasPermissionIDayOpenServiceReject { get; set; } = false;
        public bool HasPermissionIDayOpenServiceClose { get; set; } = false;
        int DayOpenDocTypeID = 137;

        CancellationTokenSource ctc = new CancellationTokenSource();

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<DayOpenRequestModel>> Select(int? NoOfRecords, int? BranchId)
        {
            return IDayOpenService.Select(NoOfRecords, BranchId);
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> Approve(DayOpenRequestModel model)
        {
            string result = "";
            HasPermissionIDayOpenServiceApprove = IUserService.GetClaimsAsync(model.UserInfo.UserID, DayOpenDocTypeID.ToString(), "Approve", model.Branch.BranchID, ctc.Token);
            if (HasPermissionIDayOpenServiceApprove)
            {
                var returnModel = IDayOpenService.Approve(model);
                if (returnModel != null)
                {
                    result = "Approved Successfully.";
                }
            }
            else
            {
                result = "Permission denied! You don't have any permission to Approve DayOpen.";
            }
            return result;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> Reject(DayOpenRequestModel model)
        {
            string result = "";
            HasPermissionIDayOpenServiceReject = IUserService.GetClaimsAsync(model.UserInfo.UserID, DayOpenDocTypeID.ToString(), "Reject", model.Branch.BranchID, ctc.Token);
            if (HasPermissionIDayOpenServiceReject)
            {
                var returnModel = IDayOpenService.RejectOrClose(model);
                if (returnModel != null)
                {
                    result = "Rejected Successfully.";
                }
            }
            else
            {
                result = "Permission denied! You don't have any permission to Reject DayOpen.";
            }
            return result;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> Close(DayOpenRequestModel model)
        {
            string result = "";
            HasPermissionIDayOpenServiceClose = IUserService.GetClaimsAsync(model.UserInfo.UserID, DayOpenDocTypeID.ToString(), "Close", model.Branch.BranchID, ctc.Token);
            if (HasPermissionIDayOpenServiceClose)
            {
                var returnModel = IDayOpenService.RejectOrClose(model);
                if (returnModel != null)
                {
                    result = "Closed Successfully.";
                }
            }
            else
            {
                result = "Permission denied! You don't have any permission to Close DayOpen.";
            }
            return result;
        }
    }
}