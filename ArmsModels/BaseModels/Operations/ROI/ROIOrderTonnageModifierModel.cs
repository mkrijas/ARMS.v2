using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        public string DisplayModifiers { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public List<ROIOrderTonnageModifierSubModel> modifiers { get; set; } = new();

    }

    public class ROIOrderTonnageModifierSubModel
    {
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public int? Modifier { get; set; }
        public virtual bool HideTextField { get; set; } = false;
    }
}