using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using Newtonsoft.Json;

namespace Core.BaseModels.Operations.ROI
{
    public class ROITonnageModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROITonnageModel>(Json);
        }
        public int? ID { get; set; }
        public OrderModel Order { get; set; } = new();
        public RouteModel Route { get; set; } = new();
        public byte? Wheels { get; set; }
        public string BSType { get; set; }
        public decimal? Tonnage { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
}