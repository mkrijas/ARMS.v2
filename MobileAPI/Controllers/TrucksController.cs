using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrucksController : ControllerBase
    {
        private readonly ITruckService _truckService;
        public TrucksController(ITruckService truckService)
        {
            _truckService = truckService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<TruckModel> SelectByBranch(int? BranchID, string Filer, string HomeOrOperation = "Operation")
        {
            return _truckService.SelectByBranch(BranchID, Filer, HomeOrOperation);
        }
    }
}
