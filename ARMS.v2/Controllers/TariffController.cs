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
    public class TariffController : ControllerBase
    {
        private readonly ILogger<TariffController> _logger;
        private ITariffService _service;

        public TariffController(ILogger<TariffController> logger, ITariffService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{TariffID}")]
        public IEnumerable<TariffModel> GetTariffs(int TariffID)
        {
            return _service.Select(TariffID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] TariffModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{TariffID}/{UserID}")]
        public IActionResult Delete(int TariffID,string UserID)
        { 
            return Ok(_service.Delete(TariffID,UserID));
        }
    }
}
