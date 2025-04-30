using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    // Model representing an operational inventory release
    public class OpInventoryReleaseModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<OpInventoryReleaseModel>(Json);
        }
        public OpInventoryReleaseModel()
        {
            NatureOfTransaction = "Op Inventory Release";
            TotalAmount = 0;
        }
        public int? OpInventoryReleaseID { get; set; } // Unique identifier for the operational inventory release
        [Required]
        public InventoryItemModel Item { get; set; } // Information about the inventory item being released
        public ChartOfAccountModel DebitCOA { get; set; } // Chart of Accounts for debit transactions
        public ChartOfAccountModel CreditCOA { get; set; } // Chart of Accounts for credit transactions
        [Required]
        public decimal? TotalQty { get; set; }
        public string Reference { get; set; }
        public bool IsUsedItem { get; set; }
        [ValidateComplexType]
        public List<OpInventoryReleaseSubModel> Items { get; set; } = new(); // List of sub-entries for the inventory release
    }

    // Model representing a sub-entry for an operational inventory release
    public class OpInventoryReleaseSubModel
    {
        public int? OpInventoryReleaseSubID { get; set; } // Unique identifier for the inventory release sub-entry
        public long? TripID { get; set; }
        public virtual string TripNo { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public virtual string TruckRegNo { get; set; }
        [Required]
        public decimal? ItemQty { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
    }
}