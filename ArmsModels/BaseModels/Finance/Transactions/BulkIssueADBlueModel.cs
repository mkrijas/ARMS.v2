using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    public class BulkIssueADBlueModel:TransactionBaseModel
    {
        public int? BulkIssueADBlueID { get; set; }
        public string BulkIssueADBlueCode { get; set; }
        public int? ItemID { get; set; }
        public InventoryItemModel Item { get; set; }
        public int? DebitCOAID { get; set; }
        public int? CreditCOAID { get; set; }
        public BulkIssueADBlueSubModel ReleaseSubDetails { get; set; }
        [ValidateComplexType]
        public List<BulkIssueADBlueSubModel> Items { get; set; } = new();
    }


    public class BulkIssueADBlueSubModel
    {
        public int? BulkIssueADBlueSubID { get; set; }
        public int? TripID { get; set; }
        public string TripNo { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public string TruckRegNo { get; set; }
        [Required]
        public decimal? ItemQty { get; set; }
    }
}
