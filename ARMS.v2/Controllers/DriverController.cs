using ArmsServices.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ARMS.v2.Controllers
{
    [ApiController]
    [Route("driver/[controller]")]
    public class DriverController : Controller
    {
        private readonly ILogger<DriverController> _logger;
        private IDriverService _service;

        public DriverController(ILogger<DriverController> logger, IDriverService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet]
        public IEnumerable<DriverModel> GetDrivers()
        {
            return _service.Select(null);
        }

        [HttpPost]
        public IActionResult UpdateDriver([FromBody] DriverModel driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_service.Update(driver));
        }

        [HttpDelete("{DriverID}/{UserID}")]
        public IActionResult DeleteDriver(int DriverID, string UserID)
        {
            return Ok(_service.Delete(DriverID, UserID));
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
