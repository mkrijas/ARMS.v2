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
    public class PartyDirectorController : ControllerBase
    {
        private readonly ILogger<PartyDirectorController> _logger;
        private IPartyDirectorService _service;

        public PartyDirectorController(ILogger<PartyDirectorController> logger, IPartyDirectorService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{PartyDirectorID}")]
        public IEnumerable<PartyDirectorModel> GetPartyDirectors(int PartyDirectorID = 0)
        {
            return _service.Select(PartyDirectorID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] PartyDirectorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{PartyDirectorID}/{UserID}")]
        public IActionResult Delete(int PartyDirectorID,string UserID)
        { 
            return Ok(_service.Delete(PartyDirectorID,UserID));
        }
    }
}
