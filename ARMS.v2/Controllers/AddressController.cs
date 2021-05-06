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
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private IAddressService _service;

        public AddressController(ILogger<AddressController> logger, IAddressService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{AddressID}")]
        public IEnumerable<AddressModel> GetAddresss(int AddressID = 0)
        {
            return _service.Select(AddressID);
        }

        [HttpPost]
        public IActionResult Update([FromBody] AddressModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(model));
        }
       

        [HttpDelete("{AddressID}/{UserID}")]
        public IActionResult Delete(int AddressID,string UserID)
        { 
            return Ok(_service.Delete(AddressID,UserID));
        }
    }
}
