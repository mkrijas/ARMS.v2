using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public OrderModel Order { get; set; }
        [Required]
        public RouteModel Route { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public string BSType { get; set; }
        [Required]
        public decimal? Tonnage { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
}