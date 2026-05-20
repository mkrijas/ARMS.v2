using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ArmsModels.BaseModels;
using ArmsServices.DataServices;

namespace GpsApiWorker
{
    public class ApiPollingService(
        ILogger<ApiPollingService> _logger, 
        OnethorApiClient _onethorApiClient, 
        ITelemetryService _truckService,
        IOptions<OnethorApiSettings> _options) : BackgroundService
    {    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("API Polling Service running.");

            var settings = _options.Value;
            if (!settings.Enabled)
            {
                _logger.LogInformation("Onethor API Polling is disabled in configuration.");
                return;
            }

            int pollingInterval = settings.PollingIntervalSeconds > 0 ? settings.PollingIntervalSeconds : 30;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Polling Onethor API for vehicle snapshots...");
                    var vehicles = await _onethorApiClient.GetAllVehicleSnapshotsAsync(stoppingToken);

                    if (vehicles != null && vehicles.Count > 0)
                    {
                        _logger.LogInformation("Fetched {Count} vehicle snapshots. Mapping to TelemetryModel...", vehicles.Count);
                        
                        var telemetryList = new List<TelemetryModel>();
                        foreach (var vt in vehicles)
                        {
                            if (string.IsNullOrWhiteSpace(vt.registrationNumber))
                            {
                                continue;
                            }
                            
                            var tm = TelemetryMapper.MapToTelemetryModel(vt);
                            telemetryList.Add(tm);
                        }

                        if (telemetryList.Count > 0)
                        {
                            _logger.LogInformation("Updating database with {Count} telemetry records...", telemetryList.Count);
                            int? res = _truckService.UpdateTelemetry(telemetryList);
                            _logger.LogInformation("Database update completed. Result: {Result}", res);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No vehicle snapshot data returned from Onethor API.");
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during periodic telemetry polling: {Message}", ex.Message);
                }

                // Wait for the specified interval before the next call
                await Task.Delay(TimeSpan.FromSeconds(pollingInterval), stoppingToken);
            }
        }
    } 
}
