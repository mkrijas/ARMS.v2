using ArmsModels.BaseModels;
using ArmsServices.DataServices;
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
        private readonly IPartyService _partyService;

        public FinanceBalanceController(IOpeningBalanceService openingBalanceService,
            IPartyService partyService)
        {
            _openingBalanceService = openingBalanceService;
            _partyService = partyService;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PartyModel> GetParty(int? BranchId)
        {
            IEnumerable<PartyModel> collection;
            collection = _partyService.SelectByBranch(BranchId).ToList();
            return collection;
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
