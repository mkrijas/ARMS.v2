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
    public class PartyController : ControllerBase
    {
        private readonly ILogger<PartyController> _logger;
        private IPartyService _service;

        public PartyController(ILogger<PartyController> logger, IPartyService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{PartyID}")]
        public IEnumerable<PartyModel> GetPartys(int PartyID = 0)
        {
            return _service.Select(PartyID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] PartyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{PartyID}/{UserID}")]
        public IActionResult Delete(int PartyID,string UserID)
        { 
            return Ok(_service.Delete(PartyID,UserID));
        }
    }
}
