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

        public DestinationFeasibilityCheckerContorller(IDestinationFeasibilityCheckerService destinationFeasibilityCheckerService,
                                                       IContentService contentService,
                                                       ITruckTypeService truckTypeService)
        {
            _destinationFeasibilityCheckerService = destinationFeasibilityCheckerService;
            _contentService = contentService;
            _truckTypeService = truckTypeService;
        }

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
        public IEnumerable<DestinationFeasibilityCheckerRatesModel> GetRates(int ID)
        {
            IEnumerable<DestinationFeasibilityCheckerRatesModel> CalculationRates;
            CalculationRates = _destinationFeasibilityCheckerService.SelectRates(ID);
            return CalculationRates;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public async Task<string> Update(DestinationFeasibilityCheckerModel updateModel)
        {
            string result = "";
            var returnModel = _destinationFeasibilityCheckerService.Update(updateModel);
            if (returnModel != null)
            {
                result = "Updated Successfully.";
            }
            return result;
        }

        //Pending GRN Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<DestinationFeasibilityCheckerModel> Select(int ID)
        {
            IEnumerable<DestinationFeasibilityCheckerModel> Collection;
            Collection = _destinationFeasibilityCheckerService.Select(ID).ToList();
            return Collection;
        }
    }
}