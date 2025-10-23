using ArmsModels.BaseModels;
using Core.IDataServices.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class FinanceBalanceController : ControllerBase
    {
        private readonly IOpeningBalanceService _openingBalanceService;

        public FinanceBalanceController(IOpeningBalanceService openingBalanceService)
        {
            _openingBalanceService = openingBalanceService;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<OpeningBalanceModel> GetBalanceByArd(string BranchIDS, string ArdCode, DateTime Date)
        {
            IEnumerable<OpeningBalanceModel> collection;
            collection = _openingBalanceService.GetBalanceByArd(BranchIDS, ArdCode, Date).ToList();
            return collection;
        }
    }
}
