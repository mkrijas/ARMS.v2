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
    [Route("Truck/[controller]")]
    public class TruckController : ControllerBase
    {
        private readonly ILogger<TruckController> _logger;
        private ITruckService _service;

        public TruckController(ILogger<TruckController> logger, ITruckService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{TruckID}")]
        public IEnumerable<TruckModel> GetTrucks(int TruckID)
        {
            return _service.Select(TruckID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] TruckModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{TruckID}/{UserID}")]
        public IActionResult Delete(int TruckID,string UserID)
        { 
            return Ok(_service.Delete(TruckID,UserID));
        }
    }
}
