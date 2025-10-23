using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using ArmsServices.DataServices.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class InvRequestController : ControllerBase
    {
        private readonly IUserService IUserService; 
                         IInventoryRequestService _inventoryRequestService;
        IInventoryItemService _inventoryItemService;

        public InvRequestController(
            IUserService userService,
            IInventoryRequestService inventoryRequestService,
            IInventoryItemService inventoryItemService)
        {
            IUserService = userService;
            _inventoryRequestService = inventoryRequestService;
            _inventoryItemService = inventoryItemService;
        }

        public bool HasPermissionEdit { get; set; } = false;
        public int DocTypeID = 67;

        CancellationTokenSource ctc = new CancellationTokenSource();

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryItemModel> GetAllItems()
        {
            IEnumerable<InventoryItemModel> collection;
            collection = _inventoryItemService.SelectByGroup(0).ToList();
            return collection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryItemEntryModel> GetInvRequestSub(int RequestID)
        {
            IEnumerable<InventoryItemEntryModel> collection;
            collection = _inventoryRequestService.GetSub(RequestID).ToList();
            return collection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateInvRequest([FromBody] InventoryRequestModel model)
        {
            HasPermissionEdit = IUserService.GetClaimsAsync(model.UserInfo.UserID, DocTypeID.ToString(), "Edit", model.BranchID, ctc.Token);
            if (HasPermissionEdit)
            {
                if (model == null)
                    return BadRequest("Invalid data.");

                try
                {
                    _inventoryRequestService.Update(model);
                    return Ok(new { success = true, message = "Saved Successfully" });
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ClosedInvRequest([FromBody] InventoryRequestModel model)
        {
            HasPermissionEdit = IUserService.GetClaimsAsync(model.UserInfo.UserID, DocTypeID.ToString(), "Edit", model.BranchID, ctc.Token);
            if (HasPermissionEdit)
            {
                if (model == null)
                    return BadRequest("Invalid data.");

                try
                {
                    _inventoryRequestService.ClosedInventory(model);
                    return Ok(new { success = true, message = "Saved Successfully" });
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteInvRequest([FromBody] InventoryRequestModel model)
        {
            HasPermissionEdit = IUserService.GetClaimsAsync(model.UserInfo.UserID, DocTypeID.ToString(), "Delete", model.BranchID, ctc.Token);
            if (HasPermissionEdit)
            {
                if (model == null)
                    return BadRequest("Invalid data.");

                try
                {
                    _inventoryRequestService.Delete(model.RequestID, model.UserInfo.UserID);
                    return Ok(new { success = true, message = "Saved Successfully" });
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
    }
}
