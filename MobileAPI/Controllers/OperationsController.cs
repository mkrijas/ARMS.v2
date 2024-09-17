using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
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

        public OperationsController(IPlaceService placeService, 
                                    IEventService eventService,
                                    IGcService gcService)
        {
            _placeService = placeService;
            _eventService = eventService;
            _gcService = gcService;
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
    }
}