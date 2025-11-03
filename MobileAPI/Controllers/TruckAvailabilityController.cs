using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Core.BaseModels.Operations;
using Core.IDataServices.Operations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class TruckAvailabilityController : ControllerBase
    {
        private readonly IUserService IUserService;
        private readonly ITruckAvailabilityService _truckAvailabilityService;
        private readonly IAssetService _assetService;
        private readonly IAssetSettingsService _assetSettingsService;

        public TruckAvailabilityController(
            IUserService iUserService,
            ITruckAvailabilityService truckAvailabilityService,
            IAssetService assetService,
            IAssetSettingsService assetSettingsService)
        {
            IUserService = iUserService;
            _truckAvailabilityService = truckAvailabilityService;
            _assetService = assetService;
            _assetSettingsService = assetSettingsService;
        }

        public bool HasPermissionEdit { get; set; } = false;
        public int DocTypeID = 139;

        CancellationTokenSource ctc = new CancellationTokenSource();

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateOutgoing(int? id, int? truckId, int? BranchId, string userId)
        {
            RequestApprovalHistoryModel model = new();
            List<int?> CurrentBranchTruckIDs = new();

            if (truckId == null)
                return BadRequest("Invalid data.");

            var result = _truckAvailabilityService.GetAllTruckIdsByBranchID(BranchId);
            if (result != null)
            {
                CurrentBranchTruckIDs = result.ToList();
            }
            if (CurrentBranchTruckIDs != null && CurrentBranchTruckIDs.Any(s => s == truckId))
            {
                return BadRequest("Truck exists in current branch.");
            }

            model.RequestApprovalHistoryID = id;
            model.TruckID = truckId;
            model.RequestedBranchID = BranchId;
            model.RequestedUserInfo.UserID = userId;
            model.RequestStatus = 0;
            try
            {
                model = _truckAvailabilityService.UpdateOutgoing(model);
                if (model.RespondedBranchID == BranchId)
                {
                    model.RespondedUserInfo = new();
                    model.RespondedBranchID = BranchId;
                    model.RequestStatus = 2;
                    model.RespondedUserInfo.UserID = userId;
                    model = _truckAvailabilityService.UpdateStatus(model, null);
                    return Ok(new { success = true, message = "Truck Returned" });
                }
                else
                    return Ok(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<RequestApprovalHistoryModel> GetAllOutGoing(int? branchId)
        {
            IEnumerable<RequestApprovalHistoryModel> collection;
            collection = _truckAvailabilityService.SelectOutgoingRequests(null, branchId).ToList().Where(x => x.RequestStatus != 2);
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<RequestApprovalHistoryModel> GetAllOutGoingCompleted(int? branchId)
        {
            IEnumerable<RequestApprovalHistoryModel> collection;
            collection = _truckAvailabilityService.SelectOutgoingRequests(null, branchId).ToList().Where(x => x.RequestStatus == 2);
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<RequestApprovalHistoryModel> GetAllIncoming(int? branchId)
        {
            IEnumerable<RequestApprovalHistoryModel> collection;
            collection = _truckAvailabilityService.SelectIncomingTrucks(null, branchId).ToList();
            return collection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RemoveOutgoing(int? Id, string userId)
        {
            if (Id == null)
                return BadRequest("Invalid data.");

            try
            {
                _truckAvailabilityService.DeleteRequest(Id, userId);
                return Ok(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<AssetSettingsModel> GetEmptyChecklist(int? truckId)
        {
            IEnumerable<AssetSettingsModel> collection;
            var asset = _assetService.SelectByTruckID(truckId);
            collection = _assetSettingsService.GetSettings(asset.SubClass.AssetSubClassID, truckId).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<AssetSettingsModel> GetChecklist(int? Id)
        {
            IEnumerable<AssetSettingsModel> collection;
            collection = _truckAvailabilityService.GetCheckList(Id).ToList();
            return collection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateChecklist([FromBody] RequestApprovalHistoryModel model)
        {
            HasPermissionEdit = IUserService.GetClaimsAsync(model.RespondedUserInfo.UserID, DocTypeID.ToString(), "Edit", model.RespondedBranchID, ctc.Token);
            if (model == null)
                return BadRequest(new { success = false, message = "Invalid data." });
            if (HasPermissionEdit)
            {
                try
                {
                    _truckAvailabilityService.UpdateCheckist(model);
                    return Ok(new { success = true, message = "Saved successfully." });
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

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Approve(int? id, int? odometer, string remarks, int? branchId, string userId)
        {
            RequestApprovalHistoryModel model = new();
            HasPermissionEdit = IUserService.GetClaimsAsync(userId, DocTypeID.ToString(), "Edit", branchId, ctc.Token);

            if (id == null)
                return BadRequest(new { success = false, message = "Invalid data." });

            if (HasPermissionEdit)
            {
                model.RequestApprovalHistoryID = id;
                model.OpeningKM = odometer;
                model.RespondedUserInfo.UserID = userId;
                model.RequestStatus = 1;
                model.Remarks = remarks;

                try
                {
                    _truckAvailabilityService.UpdateStatus(model, null);
                    return Ok(new { success = true, message = "Saved successfully." });
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

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Complete(List<AssetSettingsModel> context, int? id, int? odometer, string userId)
        {
            RequestApprovalHistoryModel model = new();
           

            if (id == null)
                return BadRequest(new { success = false, message = "Invalid data." });

            List<int?> RecievedList = context.Where(s => s.IsRecieved == true).Select(d => d.CheckListID).ToList();

            model.RequestApprovalHistoryID = id;
            model.ClosingKM = odometer;
            model.RequestStatus = 2;
            model.RespondedUserInfo.UserID = userId;

            try
            {
                _truckAvailabilityService.UpdateStatus(model, RecievedList);
                return Ok(new { success = true, message = "Saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
           
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Reject(int? id, string userId)
        {
            RequestApprovalHistoryModel model = new();

            if (id == null)
                return BadRequest(new { success = false, message = "Invalid data." });

            model.RequestApprovalHistoryID = id;
            model.RequestStatus = 99;
            model.RespondedUserInfo.UserID = userId;

            try
            {
                _truckAvailabilityService.UpdateStatus(model, null);
                return Ok(new { success = true, message = "Saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }

        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int? id, string userId)
        {
            if (id == null)
                return BadRequest(new { success = false, message = "Invalid data." });

            try
            {
                _truckAvailabilityService.DeleteRequest(id, userId);
                return Ok(new { success = true, message = "Saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

    }
}
