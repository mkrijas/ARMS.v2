using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ARMS.v2.Controllers
{
    [ApiController]
    [Route("place/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly ILogger<PlaceController> _logger;
        private IPlaceService _service;

        public PlaceController(ILogger<PlaceController> logger, IPlaceService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<PlaceModel> GetPlaces()
        {
            return _service.Select(null);
        }

        [HttpPost]       
        public IActionResult Update([FromBody] PlaceModel place)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest();
            }
            return Ok(_service.Update(place));
        }

        [HttpDelete("{PlaceID}/{UserID}")]
        public IActionResult Delete(int PlaceID,string UserID)
        { 
            return Ok(_service.Delete(PlaceID,UserID));
        }
    }
}
