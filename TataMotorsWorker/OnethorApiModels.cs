using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ArmsModels.BaseModels;

namespace TataMotorsWorker
{
    public class OnethorApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string GrantType { get; set; } = "client_credentials";
        public string TokenUrl { get; set; } = "/auth/realms/external/protocol/openid-connect/token";
        public string VehicleSnapshotUrl { get; set; } = "/api/vehicle-snapshots";
        public int PollingIntervalSeconds { get; set; } = 30;
        public bool Enabled { get; set; } = true;
    }

    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
    }

    public class VehicleTelemetry
    {
        [JsonPropertyName("vehicleId")]
        public string vehicleId { get; set; } = string.Empty;

        [JsonPropertyName("gpsLatitude")]
        public double? gpsLatitude { get; set; }

        [JsonPropertyName("gpsLongitude")]
        public double? gpsLongitude { get; set; }

        [JsonPropertyName("gpsAltitude")]
        public double? gpsAltitude { get; set; }

        [JsonPropertyName("gpsCourseInDegrees")]
        public double? gpsCourseInDeg { get; set; }

        [JsonPropertyName("gpsSignalQuality")]
        public double? gpsSignalQuality { get; set; }

        [JsonPropertyName("gpsFix")]
        public bool? gpsFix { get; set; }

        [JsonPropertyName("ignitionOn")]
        public bool? ignitionOn { get; set; }

        [JsonPropertyName("crankOn")]
        public bool? crankOn { get; set; }

        [JsonPropertyName("speed")]
        public double? speed { get; set; }

        [JsonPropertyName("odometer")]
        public double? odometer { get; set; }

        [JsonPropertyName("noOfFuelTanks")]
        public double? noOfFuelTanks { get; set; }

        [JsonPropertyName("primaryFuelLevel")]
        public double? PrimaryFuelLevel { get; set; }

        [JsonPropertyName("primaryFuelTankCapacity")]
        public double? primaryFuelTankCapacity { get; set; }

        [JsonPropertyName("secondaryFuelLevel1")]
        public double? SecondaryFuelLevel1 { get; set; }

        [JsonPropertyName("secondaryFuelTankCapacity1")]
        public double? secondaryFuelTankCapacity1 { get; set; }

        [JsonPropertyName("defLevel")]
        public double? defLevel { get; set; }

        [JsonPropertyName("backupBatteryVoltage")]
        public double? backupBatteryVoltage { get; set; }

        [JsonPropertyName("vehicleBatteryVoltage")]
        public double? vehicleBatteryVoltage { get; set; }

        [JsonPropertyName("accelX")]
        public double? accelX { get; set; }

        [JsonPropertyName("accelY")]
        public double? accelY { get; set; }

        [JsonPropertyName("accelZ")]
        public double? accelZ { get; set; }

        [JsonPropertyName("gyroX")]
        public double? gyroX { get; set; }

        [JsonPropertyName("gyroY")]
        public double? gyroY { get; set; }

        [JsonPropertyName("gyroZ")]
        public double? gyroZ { get; set; }

        [JsonPropertyName("acStatus")]
        public bool? acStatus { get; set; }

        [JsonPropertyName("vehicleStatus")]
        public string vehicleStatus { get; set; } = string.Empty;

        [JsonPropertyName("engineRunHour")]
        public double? engineRunHour { get; set; }

        [JsonPropertyName("currentGear")]
        public double? currentGear { get; set; }

        [JsonPropertyName("fuelLevelPercent")]
        public double? fuelLevelPercent { get; set; }

        [JsonPropertyName("batterySOC")]
        public List<double>? batterySOC { get; set; }

        [JsonPropertyName("noOfBatteryPacks")]
        public int? noOfBatteryPacks { get; set; }

        [JsonPropertyName("imei")]
        public string imei { get; set; } = string.Empty;

        [JsonPropertyName("registrationNumber")]
        public string registrationNumber { get; set; } = string.Empty;

        [JsonPropertyName("eventDateTime")]
        public string eventDateTime { get; set; } = string.Empty;
    }

    public class AllVehicleTelemetryResponse
    {
        [JsonPropertyName("vehicles")]
        public List<VehicleTelemetry>? vehicles { get; set; }

        [JsonPropertyName("failures")]
        public List<AllVehicleTelemetryResponseFailuresInner>? failures { get; set; }

        [JsonPropertyName("pageable")]
        public AllVehicleTelemetryResponsePageable? pageable { get; set; }
    }

    public class AllVehicleTelemetryResponseFailuresInner
    {
        [JsonPropertyName("vehicleNumber")]
        public string vehicleNumber { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public object? code { get; set; }

        [JsonPropertyName("message")]
        public string message { get; set; } = string.Empty;
    }

    public class AllVehicleTelemetryResponsePageable
    {
        [JsonPropertyName("pageNumber")]
        public int pageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int pageSize { get; set; }

        [JsonPropertyName("totalPages")]
        public int totalPages { get; set; }

        [JsonPropertyName("totalElements")]
        public int totalElements { get; set; }
    }

    public static class TelemetryMapper
    {
        public static TelemetryModel MapToTelemetryModel(VehicleTelemetry vt)
        {
            DateTime? eventTime = null;
            if (!string.IsNullOrEmpty(vt.eventDateTime))
            {
                if (DateTime.TryParse(vt.eventDateTime, out var parsedDate))
                {
                    eventTime = parsedDate;
                }
                else if (DateTime.TryParseExact(vt.eventDateTime, new[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fffZ", "yyyy-MM-ddTHH:mm:ssZ" }, 
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedExact))
                {
                    eventTime = parsedExact;
                }
            }

            if (eventTime.HasValue)
            {
                var utcTime = eventTime.Value.Kind == DateTimeKind.Utc 
                    ? eventTime.Value 
                    : DateTime.SpecifyKind(eventTime.Value, DateTimeKind.Utc);
                
                try
                {
                    var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    eventTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, istZone);
                }
                catch
                {
                    eventTime = utcTime.AddHours(5).AddMinutes(30);
                }
            }

            return new TelemetryModel
            {
                REGN_NUMBER = vt.registrationNumber,
                DATE_TIME = eventTime,
                LATITUDE = vt.gpsLatitude,
                LONGITUDE = vt.gpsLongitude,
                ALTITUDE = vt.gpsAltitude,
                SPEED = vt.speed.HasValue ? (decimal)vt.speed.Value : null,
                GEAR_NUM = vt.currentGear.HasValue ? (decimal)vt.currentGear.Value : null,
                FUEL_LEVEL = vt.PrimaryFuelLevel.HasValue && vt.primaryFuelTankCapacity.HasValue && vt.primaryFuelTankCapacity.Value > 0
                    ? (decimal)Math.Min(100.0, Math.Max(0.0, vt.PrimaryFuelLevel.Value / vt.primaryFuelTankCapacity.Value * 100.0))
                    : null,
                DEF_LEVEL = vt.defLevel.HasValue ? (decimal)vt.defLevel.Value : null,
                VEHICLE_ODO = vt.odometer.HasValue ? (decimal)vt.odometer.Value : null,
                ENGINE_SPEED = null,
                FUEL_CONS = null
            };
        }
    }
}
