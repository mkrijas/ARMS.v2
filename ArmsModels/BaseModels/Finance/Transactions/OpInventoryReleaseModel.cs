using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    public class OpInventoryReleaseModel:TransactionBaseModel
    {
        public OpInventoryReleaseModel()
        {
            NatureOfTransaction = "Op Inventory Release";
            ReceiptMode = "Cash";
            TotalAmount = 0;
        }
        public int? OpInventoryReleaseID { get; set; }
        public InventoryItemModel Item { get; set; }
        public ChartOfAccountModel DebitCOA { get; set; }
        public ChartOfAccountModel CreditCOA { get; set; }

        [Required]
        public  decimal? TotalQty { get; set; }
        [Required]
        public string ReceiptMode { get; set; }
        public string Reference { get; set; }
        [ValidateComplexType]
        public List<OpInventoryReleaseSubModel> Items { get; set; } = new();
    }


    public class OpInventoryReleaseSubModel
    {
        public int? OpInventoryReleaseSubID { get; set; }
        public long? TripID { get; set; }
        public virtual string TripNo { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public virtual string TruckRegNo { get; set; }
        [Required]
        public decimal? ItemQty { get; set; }
    }
}
