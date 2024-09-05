using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ITruckService _truckService;

        public DashboardController(IDashboardService dashboardService, ITruckService truckService)
        {
            _dashboardService = dashboardService;
            _truckService = truckService;
        }
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public List<DashboardModel> SelectTruckDonutData(int? BranchID)
        {
            return _dashboardService.SelectTruckDonutData(BranchID);
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<TruckStatusModel> GetTruckStatusByEvent(int? BranchID, string SelectedValue)
        {
            return _truckService.GetTruckStatusByEvent(BranchID, SelectedValue);
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public List<DashboardModel> SelectDriverDonutData(int? BranchID)
        {
            return _dashboardService.SelectDriverDonutData(BranchID);
        }
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<DriverModel> GetDriverStatusByEvent(int? BranchID, string SelectedValue)
        {
            return _dashboardService.GetDriverStatusByEvent(BranchID, SelectedValue);
        }
    }
}
