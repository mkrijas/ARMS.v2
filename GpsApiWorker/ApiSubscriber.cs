using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GpsApiWorker
{    
    [ApiController]
    [Route("api/[controller]")]
    public class ApiSubscriberController(ILogger<ApiPollingService> _logger, ITelemetryService _truckService) : ControllerBase
    {
        [HttpPost("/")]
        public IActionResult ReceiveData([FromBody] List<TelemetryModel> data)
        {
            if (data == null)
                return BadRequest("Invalid data");

            // Process data (store in DB, queue, etc.)
           // Console.WriteLine($"Vehicle: {data[0].REGN_NUMBER}, SPEED: {data[0].SPEED}");                    

            try
            {                
                int? res = _truckService.UpdateTelemetry(data);
                _logger.LogInformation("Updated to database successfully");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize API response.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the API response.");
            }

            return Ok(new { message = "Data received successfully" });
        }
    }
}
