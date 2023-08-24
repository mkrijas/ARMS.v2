using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;

namespace Core.BaseModels.Inventory
{
    public class StockTransferInitiationModel
    {
        public int? StockTransferID { get; set; }
        public int? InvTranID { get; set; }       
        public StoreModel InitiatedStore { get; set; }
        public BranchModel DestinationBranch { get; set; }
        public DateTime? InitiatedDate { get; set; }
        public string? Status { get; set; }
        public List<InventoryItemEntryModel> ItemsList { get; set; } = new();
        public decimal? TotalValue { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class StockTransferEndModel
    {


    }
}
