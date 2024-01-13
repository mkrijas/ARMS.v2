using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InventoryItemModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryItemModel>(Json);
        }
        public InventoryItemModel()
        {

            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? InventoryItemID { get; set; }
        [Required]
        public int? InventoryGroupID { get; set; }
        public string InventoryItemCode { get; set; }
        [Required]
        public string ItemDescription { get; set; }
        [Required]
        public string UoM { get; set; }
        [StringLength(8)]
        public string HsnCode { get; set; }
        public virtual decimal? QtyAvailable { get; set; }
        public InventoryGroupModel Group { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class InventoryGroupModel :ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryGroupModel>(Json);
        }
        public InventoryGroupModel()
        {

            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? InventoryGroupID { get; set; }
        [Required]
        public string InventoryGroupName { get; set; }
        public int? MappedPurchaseHead { get; set; }
        public int? MappedConsumptionHead { get; set; }
        public int? MappedNonInventoryPurchaseHead { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}