using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROIUnloadingChargesModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIUnloadingChargesModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public RouteModel Route { get; set; } = new();
        [Required]
        public int? Wheels { get; set; }
        [Required]
        public string BodyType { get; set; }
        [Required]
        public string RateBasis { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}