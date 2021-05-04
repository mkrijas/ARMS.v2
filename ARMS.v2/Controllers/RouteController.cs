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
    [Route("Gc/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly ILogger<RouteController> _logger;
        private IRouteService _service;

        public RouteController(ILogger<RouteController> logger, IRouteService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{RouteID}")]
        public IEnumerable<RouteModel> GetRoutes(int RouteID)
        {
            return _service.Select(RouteID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] RouteModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{RouteID}/{UserID}")]
        public IActionResult Delete(int RouteID,string UserID)
        { 
            return Ok(_service.Delete(RouteID,UserID));
        }
    }
}
