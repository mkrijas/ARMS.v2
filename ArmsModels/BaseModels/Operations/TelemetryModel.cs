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
        [JsonPropertyName("REGN_NUMBER")]
        public string REGN_NUMBER { get; set; }
        [JsonPropertyName("LATITUDE")]
        public double? LATITUDE { get; set; }
        [JsonPropertyName("LONGITUDE")]
        public double? LONGITUDE { get; set; }
        [JsonPropertyName("ALTITUDE")]
        public double? ALTITUDE { get; set; }
        [JsonPropertyName("ENGINE_SPEED")]
        public decimal? ENGINE_SPEED { get; set; }
        [JsonPropertyName("SPEED")]
        public decimal? SPEED { get; set; }
        [JsonPropertyName("GEAR_NUM")]
        public decimal? GEAR_NUM { get; set; }
        [JsonPropertyName("FUEL_LEVEL")]
        public decimal? FUEL_LEVEL { get; set; }
        [JsonPropertyName("FUEL_CONS")]
        public decimal? FUEL_CONS { get; set; }
        [JsonPropertyName("DEF_LEVEL")]
        public decimal? DEF_LEVEL { get; set; }
        [JsonPropertyName("VEHICLE_ODO")]
        public decimal? VEHICLE_ODO { get; set; }
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