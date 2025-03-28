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
        public TruckModel Truck { get; set; }
        [Required]
        public decimal? FromTonnage { get; set; }
        [Required]
        public decimal? ToTonnage { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public virtual byte? Wheels { get; set; }
        public bool HideTextField { get; set; } = false;

    }
}