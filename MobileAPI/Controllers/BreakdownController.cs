using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class BreakdownController : ControllerBase
    {
        private readonly IBreakdownService _breakdownService;
        private readonly IWorkshopService _workshopService;
        private readonly IRepairWorkService _repairWorkService;
        private readonly IMechanicService _mechanicService;

        public BreakdownController(IBreakdownService breakdownService, 
                                    IWorkshopService workshopService,
                                    IRepairWorkService repairWorkService,
                                    IMechanicService mechanicService)
        {
            _breakdownService = breakdownService;
            _workshopService = workshopService;
            _repairWorkService = repairWorkService;
            _mechanicService = mechanicService;
        }

        [HttpPost("UpdateBreakdown")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateBreakdown([FromBody] BreakdownModel model)
        {
            if (model == null)
                return BadRequest(new { success = false, message = "Invalid breakdown data." });

            try
            {
                BreakdownModel updated = _breakdownService.Update(model);

                return Ok(new
                {
                    success = true,
                    message = "Breakdown updated successfully.",
                    data = updated
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Error updating breakdown: {ex.Message}"
                });
            }
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<BreakdownModel> GetPendingBreakdown(int BranchID)
        {
            IEnumerable<BreakdownModel> collection;
            collection = _breakdownService.SelectPending(BranchID).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public BreakdownModel GetBreakdownByID(int? ID)
        {
            BreakdownModel collection;
            collection = _breakdownService.SelectByID(ID);
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<WorkshopModel> GetWorkshopList()
        {
            IEnumerable<WorkshopModel> collection;
            collection = _workshopService.Select(null).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<RepairJobModel> GetJobGroupAndSubList()
        {
            IEnumerable<RepairJobModel> collection;
            collection = _repairWorkService.SelectJobGroupAndSub().ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<RepairJobModel> GetJobList(int jobGroupId, int jobSubGroupId)
        {
            IEnumerable<RepairJobModel> collection;
            collection = _repairWorkService.SelectJob(0, jobGroupId, jobSubGroupId).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<MechanicModel> GetMechanicList(int? WorkshopID)
        {
            IEnumerable<MechanicModel> collection;
            collection = _mechanicService.SelectByWorkshop(WorkshopID).ToList();
            return collection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RejectBreakdown([FromBody] BreakdownModel request)
        {
            if (request == null || request.BreakdownID <= 0)
                return BadRequest("Invalid Breakdown ID.");

            try
            {
                var isRejected = _breakdownService.RejectBreakdown(request.BreakdownID, request.UserInfo.UserID);

                if (isRejected != null)
                    return Ok(new { success = true, message = "Breakdown rejected successfully." });
                else
                    return NotFound(new { success = false, message = "Breakdown not found or already rejected." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}