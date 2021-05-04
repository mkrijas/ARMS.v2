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
    public class GcController : ControllerBase
    {
        private readonly ILogger<GcController> _logger;
        private IGcService _service;

        public GcController(ILogger<GcController> logger, IGcService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{GcID}")]
        public IEnumerable<GcModel> GetGcs(long GcID)
        {
            return _service.Select(GcID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] GcModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{GcID}/{UserID}")]
        public IActionResult Delete(int GcID,string UserID)
        { 
            return Ok(_service.Delete(GcID,UserID));
        }
    }
}
