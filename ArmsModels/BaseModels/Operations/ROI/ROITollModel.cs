using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROITollModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROITollModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public RouteModel Route { get; set; } = new();
        [Required]
        public decimal? Toll { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}