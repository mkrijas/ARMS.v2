using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Core.BaseModels.Inventory
{ 
    public class StockTransferInitiationModel : TransactionBaseModel, ICloneable
    { 
        // Model representing the initiation of a stock transfer
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<StockTransferInitiationModel>(Json);
        }
        public int? StockTransferID { get; set; } // Unique identifier for the stock transfer
        public int? InvTranID { get; set; }
        [Required]
        public StoreModel Store { get; set; }
        public DateTime? TransferEndDate { get; set; }
        public byte? TransferCompleteStatus { get; set; }
        public string DisplayCompleteStatus
        {
            get
            {
                if (TransferCompleteStatus != 0)
                {
                    return "Completed";
                }
                else 
                { 
                    return "Rejected"; 
                }
            }
        }
        public byte? Status { get; set; }
        public string DisplayStatus
        {
            get
            {
                switch (Status)
                {
                    case 3:
                        return "Dispatched";
                    case 0:
                        return "Rejected";
                    default:
                        return "Pending";
                }
            }
        }
        public bool IsLocal { get; set; }
        public bool IsTaxable { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        [ValidateComplexType]
        [MustContain]
        public List<InventoryItemEntryModel> ItemsList { get; set; } = new();
        public StockTransferEndModel EndModel { get; set; }
    }

    // Model representing the end of a stock transfer
    public class StockTransferEndModel : TransactionBaseModel, ICloneable
    {
        public int? StockTransferEndID { get; set; } // Unique identifier for the stock transfer end
        [Required]
        public int? StockTransferID { get; set; }
        [Required]
        public StoreModel Store { get; set; }
        public int? InvTranID { get; set; }
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
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public List<InventoryItemEntryModel> ItemsList { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<StockTransferEndModel>(Json);
        }
    }
}