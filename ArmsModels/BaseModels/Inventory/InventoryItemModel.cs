using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InventoryItemModel
    {
        public int? InventoryItemID { get; set; }
        [Required]
        public int? InventoryGroupID { get; set; }
        [Required]
        [StringLength(10)]
        public string InventoryItemCode { get; set; }
        [Required]
        public string ItemDecription { get; set; }
        [StringLength(8)]
        public string HsnCode { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class InventoryGroupModel
    {
        public int? InventoryGroupID { get; set; }
        [Required]
        public string InventoryGroupName { get; set; }
        [Required]
        public int? MappedPurchaseHead { get; set; }
        [Required]
        public int? MappedConsumptionHead { get; set; }
        [Required]
        public int? MappedNonInventoryPurchaseHead { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
