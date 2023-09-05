using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InventoryItemReOrderLevelModel
    {
        public int? ID { get; set; }
        public StoreModel Store { get; set; }
        public InventoryItemModel InventoryItem { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? ReOrderLevel { get; set; }
        public decimal? InhandQty { get; set; }
        public List<InventoryItemReOrderLevelModel> ReOrderLevelList { get; set; } = new();
        public UserInfoModel UserInfo { get; set; }
    }

    public class InventoryItemReOrderLevelTableItemsModel
    {
        public int? ID { get; set; }
        public int? StoreID { get; set; }
        public int? InventoryItemID { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? ReOrderLevel { get; set; }
    }

}