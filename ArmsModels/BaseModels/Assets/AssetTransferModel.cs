using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing the initiation of an asset transfer
    public class AssetTransferInitiationModel
    {
        public int? AssetTransferID { get; set; } // Unique identifier for the asset transfer
        [Required]
        public AssetModel Asset { get; set; } // The asset being transferred
        [Required]
        public BranchModel InitiatedBranch { get; set; } // The branch initiating the transfer
        public BranchModel DestinationBranch { get; set; } // The branch receiving the asset
        public DateTime? TransferInitiatedDate { get; set; } = DateTime.Today;
        [Required]
        public string Remarks { get; set; }
        public AssetTransferEndModel AssetTransferEndModel { get; set; } = new(); // Model for the end of the asset transfer
        public UserInfoModel UserInfo { get; set; } = new();
        public int IsAssetReject { get; set; } = 0;
        [ValidateComplexType] // Validation attribute to validate complex types
        public List<AssetSettingsModel> CheckList { get; set; } // List of asset settings associated with the transfer
        public decimal? Fuel { get; set; }
        public decimal? Expenses { get; set; }
        public int? MID { get; set; }
        public string DocumentNumber { get; set; }
        public string GstMechanism { get; set; } = "FCM";
        public virtual string Images { get; set; } = "";
        public virtual List<string> ImagePath { get; set; } = new List<string>();
        public List<AssetTransferItemModel> Items { get; set; } = new();
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }

    }

    // Model representing the end of an asset transfer
    public class AssetTransferEndModel
    {
        public int? AssetTransferEndID { get; set; } // Unique identifier for the end of the asset transfer
        public int? BranchID { get; set; }
        public bool? TransferStatus { get; set; }
        public DateTime? TransferEndDate { get; set; } = DateTime.Today;
        public string DocumentNumber { get; set; }
        public string GstMechanism { get; set; } = "FCM";
        [Required]
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }

        // Property to get the status text based on the transfer status
        public string StatusText 
        {
            get
            {
                if (TransferStatus == null)
                    return "Being Transferred";
                else if (TransferStatus.Value)
                    return "Completed";
                else
                    return "Rejected";
            }
        }
    }

    public class AssetTransferItemModel
    {
        public int? ID { get; set; }
        public int? AssetID { get; set; }
        public decimal? BookValue { get; set; }
        public decimal? TaxableValue { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? TaxValue { get; set; }
        public virtual string Description { get; set; }
        public virtual string AssetStatus { get; set; }
    }

}
