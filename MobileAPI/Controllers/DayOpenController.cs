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
        public DayOpenController(IDayOpenService _IDayOpenService)
        {
            IDayOpenService = _IDayOpenService;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<DayOpenRequestModel>> Select(int? NoOfRecords, int? BranchId)
        {
            return IDayOpenService.Select(NoOfRecords, BranchId);
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<DayOpenRequestModel> Approve(DayOpenRequestModel model)
        {
            return IDayOpenService.Approve(model);
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<DayOpenRequestModel> RejectOrClose(DayOpenRequestModel model)
        {
            return IDayOpenService.RejectOrClose(model);
        }
    }
}