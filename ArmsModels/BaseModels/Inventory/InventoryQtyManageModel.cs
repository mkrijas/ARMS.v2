using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{ 
    public class InventoryQtyManageModel
    {
        public int? QtyManageID { get; set; }
        public StoreModel Store { get; set; }
        public InventoryItemModel InventoryItem { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? ReOrderQty { get; set; }
        public decimal? InhandQty { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
