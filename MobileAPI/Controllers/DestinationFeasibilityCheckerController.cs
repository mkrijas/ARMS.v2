using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using ArmsServices.DataServices.Finance.Transactions;
using Core.BaseModels.Finance.Transactions;
using Core.BaseModels.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MobileAPI.Services;
using System;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class DestinationFeasibilityCheckerContorller : ControllerBase
    {
        private readonly IDestinationFeasibilityCheckerService _destinationFeasibilityCheckerService;
        private readonly IContentService _contentService;
        private readonly ITruckTypeService _truckTypeService;
        private readonly IUserService _userService;

        public DestinationFeasibilityCheckerContorller(IDestinationFeasibilityCheckerService destinationFeasibilityCheckerService,
                                                       IContentService contentService,
                                                       ITruckTypeService truckTypeService,
                                                       IUserService userService)
        {
            _destinationFeasibilityCheckerService = destinationFeasibilityCheckerService;
            _contentService = contentService;
            _truckTypeService = truckTypeService;
            _userService = userService;
        }
        public bool HasPermissionDfcServiceEdit { get; set; } = false;
        public int DfcDoctypeId = 144;

        CancellationTokenSource ctc = new CancellationTokenSource();

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IAsyncEnumerable<ContentModel> GetContentLists()
        {
            IAsyncEnumerable<ContentModel> ContentLists;
            ContentLists = _contentService.Select(0);
            return ContentLists;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<TruckTypeModel> GetTruckTypeLists()
        {
            IEnumerable<TruckTypeModel> TruckTypeLists;
            TruckTypeLists = _truckTypeService.Select(0);
            return TruckTypeLists;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public DestinationFeasibilityCheckerRatesModel GetRates(int ID)
        {
            DestinationFeasibilityCheckerRatesModel CalculationRates;
            CalculationRates = _destinationFeasibilityCheckerService.SelectRates(ID);
            return CalculationRates;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(DestinationFeasibilityCheckerModel updateModel)
        {
            HasPermissionDfcServiceEdit = _userService.GetClaimsAsync(updateModel.UserInfo.UserID, DfcDoctypeId.ToString(), "Edit", updateModel.BranchID, ctc.Token);
            if (HasPermissionDfcServiceEdit)
            {                
                var returnModel = _destinationFeasibilityCheckerService.Update(updateModel);
                if (returnModel != null)
                {
                    return Ok("Saved Successfully.");
                }
            }
            return BadRequest("Permission denied! You don't have any permission to Edit Place.");
        }

        //Pending GRN Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<DestinationFeasibilityCheckerModel> Select(int ID, int BranchID)
        {
            IEnumerable<DestinationFeasibilityCheckerModel> Collection;
            Collection = _destinationFeasibilityCheckerService.Select(ID, BranchID).ToList();
            return Collection;
        }
    }
}