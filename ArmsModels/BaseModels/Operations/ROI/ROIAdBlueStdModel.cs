using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROIAdBlueStdModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIMileageModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public string BSType { get; set; }       
        [Required]
        public decimal? AdBlueRatio { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}
