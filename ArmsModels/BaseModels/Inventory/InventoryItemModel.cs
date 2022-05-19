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
        public InventoryItemModel()
        {

            UserInfo = new SharedModels.UserInfoModel();
        }
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
