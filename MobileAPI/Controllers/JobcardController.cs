using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using ArmsServices.DataServices.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class JobcardController : ControllerBase
    {
        private readonly IUserService IUserService;
        private readonly IJobcardService _jobcardService;
        private readonly IJobcardWorkshopService _jobcardWorkshopService;
        private readonly IJobInProgressService _jobInProgressService;
        private readonly IMechanicJobService _mechanicJobService;
        private readonly IInventoryRequestService _inventoryRequestService;

        public JobcardController(
            IUserService iUserService,
            IJobcardService jobcardService,
            IJobcardWorkshopService jobcardWorkshopService,
            IJobInProgressService jobInProgressService,
            IMechanicJobService mechanicJobService,
            IInventoryRequestService inventoryRequestService)
        {
            IUserService = iUserService;
            _jobcardService = jobcardService;
            _jobcardWorkshopService = jobcardWorkshopService;
            _jobInProgressService = jobInProgressService;
            _mechanicJobService = mechanicJobService;
            _inventoryRequestService = inventoryRequestService;
        }

        public bool HasPermissionEdit { get; set; } = false;
        public int DocTypeID = 82;

        CancellationTokenSource ctc = new CancellationTokenSource();

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateJobcard([FromBody] JobcardModel model)
        {
            HasPermissionEdit = IUserService.GetClaimsAsync(model.UserInfo.UserID, DocTypeID.ToString(), "Edit", model.BranchID, ctc.Token);
            if (HasPermissionEdit)
            {
                if (model == null)
                    return BadRequest("Invalid jobcard data.");

                try
                {
                    var updatedJobcard = _jobcardService.Update(model);
                    model.JobcardID = updatedJobcard.JobcardID;

                    // --- Link Workshop ---
                    JobcardWorkshopModel Jwm = new()
                    {
                        WorkshopID = int.Parse(model.workshop),
                        JobCardID = model.JobcardID,
                        EnteredOn = DateTime.Now,
                        UserInfo = model.UserInfo,
                        Odometer = model.Odometer
                    };
                    _jobcardWorkshopService.Update(Jwm);

                    // Save Jobs
                    if (model.Jobs != null && model.Jobs.Count > 0)
                    {
                        foreach (var job in model.Jobs)
                        {
                            job.JobCardID = model.JobcardID;
                            job.WorkshopID = int.Parse(model.workshop);
                            job.UserInfo = model.UserInfo;
                            job.Odometer = model.Odometer;
                            UpdateJob(job);
                        }
                    }

                    return Ok(new { success = true, message = "Saved Successfully", jobcardId = model.JobcardID });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = ex.Message });
                }
            }
            else
            {
                return BadRequest(new { Status = "Permission denied!" });
            }
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<JobcardModel> GetActiveJobcard(int BranchID)
        {
            IEnumerable<JobcardModel> collection;
            collection = _jobcardService.SelectByBranch(BranchID, true).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<JobcardWorkshopModel> GetWorkshopByJobcard(int? JobcardID)
        {
            IEnumerable<JobcardWorkshopModel> collection;
            collection = _jobcardWorkshopService.SelectByJobcard(JobcardID).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<JobInProgressModel> GetJobListByJobcard(int? JobcardID)
        {
            IEnumerable<JobInProgressModel> collection;
            collection = _jobInProgressService.SelectByJobcard(JobcardID).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<MechanicJobModel> GetMechanicsByJob(int? JobID)
        {
            IEnumerable<MechanicJobModel> collection;
            collection = _mechanicJobService.SelectByJob(JobID).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryRequestModel> GetInvRequestListByJobcard(int? JobcardID)
        {
            IEnumerable<InventoryRequestModel> collection;
            collection = _inventoryRequestService.SelectRequestReleaseByJobCardID(JobcardID).ToList();
            return collection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateJobcardWorkshop([FromBody] JobcardWorkshopModel model)
        {
            if (model == null)
                    return BadRequest("Invalid data.");

            try
            {
                _jobcardWorkshopService.Update(model);
                return Ok(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }            
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateJobStatus([FromBody] JobInProgressModel model, int status)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            try
            {
                model.JobStatus = status;
                model.FinishedOn = DateTime.Now;
                foreach (var item in model.Mechanics)
                {
                    RemoveMechanics(item);
                }
                _jobInProgressService.Update(model);
                return Ok(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateJob([FromBody] JobInProgressModel model)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            try
            {
                var Createdjob = _jobInProgressService.Update(model);
                foreach (var item in model.Mechanics)
                {
                    item.JipID = Createdjob.JipID;
                    item.UserInfo.UserID = model.UserInfo.UserID;
                    _mechanicJobService.Update(item);
                }
                return Ok(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
                
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RemoveMechanics([FromBody] MechanicJobModel model)
        {
            if (model == null)
                return BadRequest("No mechanics to remove.");

            try
            {                 
                _mechanicJobService.Remove(model.MjID, model.UserInfo.UserID);
                return Ok(new { success = true, message = "Mechanics removed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CloseJobcard(int? JobcardID, string UserID)
        {            
            try
            {
                _jobcardService.CloseJobcard(JobcardID, UserID);
                return Ok(new { success = true, message = "Jobcard closed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
