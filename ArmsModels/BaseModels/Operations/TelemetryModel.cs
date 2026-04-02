using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmsModels.BaseModels
{
    public class TelemetryModel
    {
        [JsonPropertyName("DATE_TIME")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? DATE_TIME { get; set; }
        public string REGN_NUMBER { get; set; }
        public double? LATITUDE { get; set; }
        public double? LONGITUDE { get; set; }
        public double? ALTITUDE { get; set; }
        public decimal? ENGINE_SPEED { get; set; }
        public decimal? SPEED { get; set; }
        public decimal? GEAR_NUM { get; set; }
        public decimal? FUEL_LEVEL { get; set; }
        public decimal? FUEL_CONS { get; set; }
        public decimal? DEF_LEVEL { get; set; }
    }

    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, _format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}