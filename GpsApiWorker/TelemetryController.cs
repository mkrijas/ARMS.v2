using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsApiWorker
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController(
        ITelemetryService telemetryService, 
        OnethorApiClient onethorApiClient, 
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

        [HttpGet("live/{registrationNumber}")]
        public async Task<IActionResult> GetLive(string registrationNumber)
        {
            try
            {
                var vt = await onethorApiClient.GetVehicleSnapshotAsync(registrationNumber);
                if (vt == null)
                    return NotFound(new { message = $"No live data found for registration number: {registrationNumber}" });
                
                var mapped = TelemetryMapper.MapToTelemetryModel(vt);
                return Ok(mapped);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving live telemetry for registration number {RegNo}", registrationNumber);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
