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
    public class TruckTypeController : ControllerBase
    {
        private readonly ILogger<TruckTypeController> _logger;
        private ITruckTypeService _service;

        public TruckTypeController(ILogger<TruckTypeController> logger, ITruckTypeService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<TruckTypeModel> GetTruckTypes()
        {
            return _service.Select(null);
        }

        [HttpPost]
        public IActionResult Update([FromBody] TruckTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{TruckTypeID}/{UserID}")]
        public IActionResult Delete(int TruckTypeID,string UserID)
        { 
            return Ok(_service.Delete(TruckTypeID,UserID));
        }
    }
}
