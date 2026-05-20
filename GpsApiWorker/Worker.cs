using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Microsoft.Data.SqlClient;

namespace GpsApiWorker
{
    public class ApiPollingService(
        ILogger<ApiPollingService> _logger, 
        OnethorApiClient _onethorApiClient, 
        ITelemetryService _truckService,
        IDbService _dbService,
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
                        
                        // Load VIN to RegNo mapping from the database for resolving license plates
                        var vinToRegMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        try
                        {
                            var records = _dbService.QuerySql(@"
                                SELECT t.ChassisNumber, r.RegNo 
                                FROM [dbo].[Truck.Trucks] t (NOLOCK)
                                LEFT JOIN [dbo].[Truck.Registration] r (NOLOCK) ON t.TruckID = r.TruckID
                                LEFT JOIN [dbo].[Truck.Types] tp (NOLOCK) ON t.TruckTypeID = tp.TruckTypeID
                                WHERE t.ChassisNumber IS NOT NULL AND r.RegNo IS NOT NULL AND tp.TruckType LIKE '%TATA%'", new List<SqlParameter>());

                            foreach (var dr in records)
                            {
                                string chassis = dr["ChassisNumber"]?.ToString()?.Trim();
                                string regNo = dr["RegNo"]?.ToString()?.Trim();
                                if (!string.IsNullOrEmpty(chassis) && !string.IsNullOrEmpty(regNo))
                                {
                                    vinToRegMap[chassis] = regNo;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to load truck VIN mapping from DB: {Message}", ex.Message);
                        }

                        var telemetryList = new List<TelemetryModel>();
                        foreach (var vt in vehicles)
                        {
                            var tm = TelemetryMapper.MapToTelemetryModel(vt);
                            
                            string vehicleId = vt.vehicleId?.Trim();
                            string rawRegNo = vt.registrationNumber?.Trim();
                            string resolvedRegNo = null;

                            // 1. Check if vehicleId (VIN) exists in our DB lookup map
                            if (!string.IsNullOrEmpty(vehicleId) && vinToRegMap.TryGetValue(vehicleId, out var regFromVin))
                            {
                                resolvedRegNo = regFromVin;
                            }
                            // 2. Check if the registrationNumber field itself contains a VIN
                            else if (!string.IsNullOrEmpty(rawRegNo) && vinToRegMap.TryGetValue(rawRegNo, out var regFromRawReg))
                            {
                                resolvedRegNo = regFromRawReg;
                            }
                            // 3. Fallback to raw registrationNumber
                            else if (!string.IsNullOrEmpty(rawRegNo))
                            {
                                resolvedRegNo = rawRegNo;
                            }
                            // 4. Ultimate fallback to vehicleId
                            else if (!string.IsNullOrEmpty(vehicleId))
                            {
                                resolvedRegNo = vehicleId;
                            }

                            if (string.IsNullOrWhiteSpace(resolvedRegNo))
                            {
                                continue;
                            }

                            tm.REGN_NUMBER = resolvedRegNo;
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
