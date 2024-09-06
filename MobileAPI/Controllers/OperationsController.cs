using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        private readonly IEventService _eventService;
        public OperationsController(IPlaceService placeService, 
                                    IEventService eventService)
        {
            _placeService = placeService;
            _eventService = eventService;
        }

        //Place Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PlaceModel> PlaceSelect(string PlaceLike)
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
    }
}
