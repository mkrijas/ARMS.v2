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
    [Route("Operation/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ILogger<TripController> _logger;
        private ITripService _service;

        public TripController(ILogger<TripController> logger, ITripService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<TripModel> GetTrips()
        {
            return _service.Select(null);
        }

        [HttpPost]
        public IActionResult Update([FromBody] TripModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{TripID}/{UserID}")]
        public IActionResult Delete(int TripID,string UserID)
        { 
            return Ok(_service.Delete(TripID,UserID));
        }
    }
}
