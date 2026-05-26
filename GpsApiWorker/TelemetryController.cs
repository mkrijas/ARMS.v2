using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace GpsApiWorker
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController(
        ITelemetryService telemetryService, 
        ILogger<TelemetryController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string? registrationNumber, [FromQuery] DateTime? dateTime)
        {
            try
            {
                var data = telemetryService.GetTelemetry(dateTime, registrationNumber ?? string.Empty);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving telemetry from database");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
