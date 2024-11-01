using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

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
        public byte? Wheels { get; set; }
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
        public OrderModel Order { get; set; } = new();
        public decimal? LoadingDuration { get; set; }
        public decimal? UnLoadingDuration { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
}