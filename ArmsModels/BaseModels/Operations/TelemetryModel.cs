using System;
using System.Collections.Generic;

namespace ArmsModels.BaseModels
{
    public class TelemetryModel
    {       
        public DateTime? DATE_TIME { get; set; }
        public string REGN_NUMBER { get; set; }
        public double? LATITUDE { get; set; }
        public double? LONGITUDE { get; set; }
        public double? ALTITUDE { get; set; }
        public decimal? ENGINE_SPEED { get; set; }
        public decimal? SPEED { get; set; }
        public int? GEAR_NUM { get; set; }
        public decimal? FUEL_LEVEL { get; set; }
        public decimal? FUEL_CONS { get; set; }
        public decimal? DEF_LEVEL { get; set; }
    }
}
