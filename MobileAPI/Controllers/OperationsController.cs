using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Core.BaseModels.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OperationsController(IPlaceService placeService, 
                                    IEventService eventService,
                                    IGcService gcService,
                                    IOrderService orderService,
                                    IRouteService routeService)
        {
            _placeService = placeService;
            _eventService = eventService;
            _gcService = gcService;
            _orderService = orderService;
            _routeService = routeService;
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
        public async Task<string> Update(OrderModel updateModel)
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
        public IAsyncEnumerable<OrderModel> SelectOrders(int BranchID)
        {
            IAsyncEnumerable<OrderModel> OrderCollection;
            OrderCollection = _orderService.SelectByBranch(BranchID);
            return OrderCollection;
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> Update(RouteModel updateModel)
        {
            string result = "";
            var returnModel = _routeService.Update(updateModel);
            if (returnModel != null)
            {
                result = "Updated Successfully.";
            }
            return result;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IAsyncEnumerable<RouteModel> SelectRoutes(int RoutID)
        {
            IAsyncEnumerable<RouteModel> RouteCollection;
            RouteCollection = _routeService.Select(0);
            return RouteCollection;
        }
    }
}