using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Model representing the reorder level for inventory items
    public class InventoryItemReOrderLevelModel
    {
        public int? ID { get; set; } // Unique identifier for the reorder level entry
        public StoreModel Store { get; set; } // Associated store for the inventory item
        public InventoryItemModel InventoryItem { get; set; } // Associated inventory item
        public decimal? MinQty { get; set; }
        public decimal? ReOrderLevel { get; set; }
        public decimal? InhandQty { get; set; }
        public List<InventoryItemReOrderLevelModel> ReOrderLevelList { get; set; } = new(); // List of reorder levels
        public UserInfoModel UserInfo { get; set; }
    }

    // Model representing a table item for inventory item reorder levels
    public class InventoryItemReOrderLevelTableItemsModel
    {
        public int? ID { get; set; }
        public int? StoreID { get; set; }
        public int? InventoryItemID { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? ReOrderLevel { get; set; }
    }

}