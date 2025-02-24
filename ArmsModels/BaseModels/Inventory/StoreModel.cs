using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace ArmsModels.BaseModels
{
    // Represents a store model with properties related to the store's information
    public class StoreModel : ICloneable
    {
        public StoreModel(string branchName)
        {
            UserInfo = new();
            BranchName = branchName;
        }
        public StoreModel()
        {
            UserInfo = new();

        }

        public int? StoreID { get; set; } // Unique identifier for the store (nullable)
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public virtual string BranchName { get; internal set; }
        public UserInfoModel UserInfo { get; set; }
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<StoreModel>(Json);
        }
    }

    // Represents a batch of inventory items
    public class InventoryBatchModel
    {
        public long? BatchID { get; set; } // Unique identifier for the batch (nullable)
        public int? StoreID { get; set; }
        public int? ItemID { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? PurchaseQty { get; set; }
        public decimal? InhandQty { get; set; }
    }

    // Represents a batch that can be linked
    public class LinkableBatchModel
    {
        public long? BatchID { get; set; }
        public int? StoreID { get; set; }
        public decimal? LinkableQty { get; set; }
        public int? PartyID { get; set; }
        public string GrnNo { get; set; }
    }
}
