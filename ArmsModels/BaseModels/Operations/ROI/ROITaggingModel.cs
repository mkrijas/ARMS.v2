using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROITaggingModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROITaggingModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public string BodyType { get; set; }
        public OrderModel Order { get; set; } = new();
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}