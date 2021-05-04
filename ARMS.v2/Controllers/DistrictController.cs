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
    public class DistrictController : ControllerBase
    {
        private readonly ILogger<DistrictController> _logger;
        private IDistrictService _service;

        public DistrictController(ILogger<DistrictController> logger, IDistrictService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<DistrictModel> GetDistricts()
        {
            return _service.Select(null);
        }

        [HttpPost]       
        public IActionResult Update([FromBody] DistrictModel model)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }

        [HttpDelete("{DistrictID}/{UserID}")]
        public IActionResult Delete(int DistrictID,string UserID)
        { 
            return Ok(_service.Delete(DistrictID,UserID));
        }
    }
}
