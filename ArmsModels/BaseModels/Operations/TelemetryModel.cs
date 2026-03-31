using System;
using System.Collections.Generic;

namespace ArmsModels.BaseModels
{
    public class TelemetryModel
    {
        public int? ID { get; set; }
        public string VehicleId { get; set; }
            public double? GpsLatitude { get; set; }
            public double? GpsLongitude { get; set; }
            public int? GpsAltitude { get; set; }
            public int? GpsCourseInDegrees { get; set; }
            public int? GpsSignalQuality { get; set; }
            public bool? GpsFix { get; set; }
            public bool? IgnitionOn { get; set; }
            public bool? CrankOn { get; set; }
            public int? Speed { get; set; }
            public int? Odometer { get; set; }
            public int? NoOfFuelTanks { get; set; }
            public int? PrimaryFuelLevel { get; set; }
            public int? PrimaryFuelTankCapacity { get; set; }
            public int? SecondaryFuelLevel1 { get; set; }
            public int? SecondaryFuelTankCapacity1 { get; set; }
            public int? DefLevel { get; set; }
            public double? BackupBatteryVoltage { get; set; }
            public double? VehicleBatteryVoltage { get; set; }
            public double? AccelX { get; set; }
            public double? AccelY { get; set; }
            public double? AccelZ { get; set; }
            public double? GyroX { get; set; }
            public double? GyroY { get; set; }
            public double? GyroZ { get; set; }
            public bool? AcStatus { get; set; }
            public string VehicleStatus { get; set; }
            public double? EngineRunHour { get; set; }
            public double? CurrentGear { get; set; }
            public double? FuelLevelPercent { get; set; }
            public List<double> BatterySOC { get; set; } = [];
            public int? NoOfBatteryPacks { get; set; }
            public string Imei { get; set; }
            public string RegistrationNumber { get; set; }
            public DateTime? EventDateTime { get; set; }        
    }
}
