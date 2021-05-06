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
    [Route("Entity/[controller]")]
    public class GstController : ControllerBase
    {
        private readonly ILogger<GstController> _logger;
        private IGstService _service;

        public GstController(ILogger<GstController> logger, IGstService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{GstID}")]
        public IEnumerable<GstModel> GetGsts(int GstID = 0)
        {
            return _service.Select(GstID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] GstModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{GstID}/{UserID}")]
        public IActionResult Delete(int GstID,string UserID)
        { 
            return Ok(_service.Delete(GstID,UserID));
        }
    }
}
