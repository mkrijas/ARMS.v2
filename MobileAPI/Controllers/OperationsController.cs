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

        public OperationsController(IPlaceService placeService,
                                    IEventService eventService,
                                    IGcService gcService,
                                    IOrderService orderService,
                                    IRouteService routeService,
                                    IDistrictService districtService)
        {
            _placeService = placeService;
            _eventService = eventService;
            _gcService = gcService;
            _orderService = orderService;
            _routeService = routeService;
            _districtService = districtService;
        }

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
        public async Task<string> UpdateOrder(OrderModel updateModel)
        {
            string result = "";
            var returnModel = _orderService.Update(updateModel);
            if (returnModel != null)
            {
                result = "Updated Successfully.";
            }
            return result;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<OrderModel>> SelectOrders(int BranchID)
        {
            var OrderCollection = new List<OrderModel>();
            await foreach (var order in _orderService.SelectByBranch(BranchID))
            {
                OrderCollection.Add(order);
            }
            return OrderCollection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]        
        public async Task<IActionResult> UpdateRoute(RouteModel updateModel)
        {
            var returnModel = await _routeService.Update(updateModel);
            if (returnModel != null)
            {
                return Ok("Updated Successfully.");
            }
            return BadRequest("Update failed.");
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
            var returnModel = _districtService.Update(updateModel);
            if (returnModel != null)
            {
                return Ok("Updated Successfully.");
            }
            return BadRequest("Update failed.");
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
            var returnModel = await _placeService.Update(updateModel);
            if (returnModel != null)
            {
                return Ok("Updated Successfully.");
            }
            return BadRequest("Update failed.");
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PlaceModel> SelectPlaces(int? PlaceID, string PlaceLike)
        {
            IEnumerable<PlaceModel> PlaceCollection;
            PlaceCollection = _placeService.Select(PlaceID, PlaceLike).ToList();
            return PlaceCollection;
        }
    }
}