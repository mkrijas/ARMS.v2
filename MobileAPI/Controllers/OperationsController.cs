using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Core.BaseModels.Operations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Numerics;
using System.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        private readonly IEventService _eventService;
        private readonly IGcService _gcService;
        private readonly IOrderService _orderService;
        private readonly IRouteService _routeService;
        private readonly IDistrictService _districtService;
        private readonly IUserService _userService;

        public OperationsController(IPlaceService placeService,
                                    IEventService eventService,
                                    IGcService gcService,
                                    IOrderService orderService,
                                    IRouteService routeService,
                                    IDistrictService districtService,
                                    IUserService userService)
        {
            _placeService = placeService;
            _eventService = eventService;
            _gcService = gcService;
            _orderService = orderService;
            _routeService = routeService;
            _districtService = districtService;
            _userService = userService;
        }
        public bool HasPermissionDistrictServiceEdit { get; set; } = false;
        public int DistrictDocTypeID = 54;
        public bool HasPermissionPlaceServiceEdit { get; set; } = false;
        public int PlaceDocTypeID = 44;
        public bool HasPermissionRouteServiceEdit { get; set; } = false;
        public int RouteDocTypeID = 43;

        CancellationTokenSource ctc = new CancellationTokenSource();

        //Place Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PlaceModel> PlaceSelect(string? PlaceLike)
        {
            return _placeService.Select(0, PlaceLike);
        }

        //EventType Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<EventTypeModel> EventTypesSelect()
        {
            IEnumerable<EventTypeModel> EventTypes;
            EventTypes = _eventService.GetEventTypes().Where(x => x.EventTypeID != 1 && x.IsTripRelated).ToList();
            return EventTypes;
        }

        //GC List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<GcSetModel> GCSelect(int? TripID)
        {
            IEnumerable<GcSetModel> GCsToUnload;
            GCsToUnload = _gcService.SelectToUnload(TripID).ToList();
            return GCsToUnload;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<GcSetModel> GCSelectUnloaded(int? TripID)
        {
            IEnumerable<GcSetModel> GCsToUnload;
            GCsToUnload = _gcService.SelectUnloadedMobile(TripID).ToList();
            return GCsToUnload;
        }
        
        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]        
        public async Task<IActionResult> UpdateRoute(RouteModel updateModel)
        {
            HasPermissionRouteServiceEdit = _userService.GetClaimsAsync(updateModel.UserInfo.UserID, RouteDocTypeID.ToString(), "Edit", 7, ctc.Token);
            if (HasPermissionRouteServiceEdit)
            {
                var returnModel = await _routeService.Update(updateModel);
                if (returnModel != null)
                {
                    return Ok("Saved Successfully.");
                }
            }
            return BadRequest("Permission denied! You don't have any permission to Edit Route.");                   
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<RouteModel>> SelectRoutes(int RoutID)
        {
            var RouteCollection = new List<RouteModel>();
            await foreach (var route in _routeService.Select(0))
            {
                RouteCollection.Add(route);
            }
            return RouteCollection;            
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<StateModel> SelectSatates()
        {
            IEnumerable<StateModel> StateCollection;
            StateCollection = _districtService.GetStates().ToList();
            return StateCollection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateDistrict(DistrictModel updateModel)
        {
            HasPermissionDistrictServiceEdit = _userService.GetClaimsAsync(updateModel.UserInfo.UserID, DistrictDocTypeID.ToString(), "Edit", 7, ctc.Token);
            if (HasPermissionDistrictServiceEdit)
            {
                var returnModel = _districtService.Update(updateModel);
                if (returnModel != null)
                {
                    return Ok("Saved Successfully.");
                }
            }
            return BadRequest("Permission denied! You don't have any permission to Edit District.");
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<DistrictModel> SelectDistricts(int? DistrictID)
        {
            IEnumerable<DistrictModel> DistrictCollection;
            DistrictCollection = _districtService.Select(DistrictID).ToList();
            return DistrictCollection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePlace(PlaceModel updateModel)
        {
            HasPermissionPlaceServiceEdit = _userService.GetClaimsAsync(updateModel.UserInfo.UserID, PlaceDocTypeID.ToString(), "Edit", 7, ctc.Token);
            if (HasPermissionPlaceServiceEdit)
            {
                var returnModel = await _placeService.Update(updateModel);
                if (returnModel != null)
                {
                    return Ok("Saved Successfully.");
                }
            }
            return BadRequest("Permission denied! You don't have any permission to Edit Place.");
        }
    }
}