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
    public class StateController : ControllerBase
    {
        private readonly ILogger<StateController> _logger;
        private IStateService _service;

        public StateController(ILogger<StateController> logger, IStateService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<StateModel> GetStates()
        {
            return _service.Select(null);
        }

        [HttpPost]       
        public IActionResult Update([FromBody] StateModel model)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }

        [HttpDelete("{StateID}/{UserID}")]
        public IActionResult Delete(int StateID,string UserID)
        { 
            return Ok(_service.Delete(StateID,UserID));
        }
    }
}
