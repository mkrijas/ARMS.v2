using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Operations.ROI
{
    public class ROIOrderTonnageModifierModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIOrderTonnageModifierModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public OrderModel Order { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public int? Modifier { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public bool HideTextField { get; set; }

    }
}