using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Operations.ROI
{
    public class ROIWheelSpeedModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIWheelSpeedModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public decimal? Speed { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class ROILoadAndUnloadModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROILoadAndUnloadModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public OrderModel Order { get; set; } = new();
        [Required]
        public decimal? LoadingDuration { get; set; }
        [Required]
        public decimal? UnLoadingDuration { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
}