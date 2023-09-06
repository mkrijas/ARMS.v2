using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.BaseModels.Inventory
{
    public class StockTransferInitiationModel
    {
        public int? StockTransferID { get; set; }
        public int? InvTranID { get; set; }
        public StoreModel Store { get; set; }
        public BranchModel Branch { get; set; }
        public DateTime? InitiatedDate { get; set; }
        public byte? Status { get; set; }
        public string? DisplayStatus
        {
            get
            {
                switch (Status)
                {
                    case 3:
                        return "Dispatched";
                    case 0:
                        return "Cancelled";
                    default:
                        return "Pending";
                }
            }
        }
        public List<InventoryItemEntryModel> ItemsList { get; set; } = new();
        public decimal? TotalValue { get { return ItemsList.Sum(X => X.ItemQty * X.ItemRate);  } }
        public StockTransferEndModel EndModel { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class StockTransferEndModel
    {
        public int? StockTransferEndID { get; set; }
        //public StoreModel DestinationStore { get; set; }
        //public BranchModel DestinationBranch { get; set; }
        public byte? TransferStatus { get; set; }
        public string? DisplayTransferStatus
        {
            get
            {
                switch (TransferStatus)
                {
                    case 3:
                        return "Accepted";
                    case 0:
                        return "Rejected";
                    default:
                        return "Pending";
                }
            }
        }
        public DateTime? TransferEndDate { get; set; }
        public byte? RecordStatus { get; set; }
        public List<InventoryItemEntryModel> ItemsEndList { get; set; } = new();

    }
}
