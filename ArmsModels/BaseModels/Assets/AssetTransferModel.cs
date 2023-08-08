using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class AssetTransferInitiationModel
    {
        public int? AssetTransferID { get; set; }
        [Required]
        public AssetModel Asset { get; set; }
        [Required]
        public BranchModel InitiatedBranch { get; set; }
        public BranchModel DestinationBranch { get; set; }
        public DateTime? TransferInitiatedDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
        public AssetTransferEndModel AssetTransferEndModel { get; set; }
        public int IsAssetReject { get; set; } = 0;
        public List<AssetTransferCheckListModel> CheckList { get; set; } = new();

    }
    public class AssetTransferEndModel
    {
        public int? AssetTransferEndID { get; set; }
        public int? BranchID { get; set; }
        public bool? TransferStatus { get; set; }
        public DateTime? TransferEndDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
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

    public class AssetTransferCheckListModel
    {
        public int? CheckListID { get; set; }
        public int? AssetTransferID { get; set; }
        [Required]
        public int? AssetSettingsID { get; set; }
        [Required]
        public string Description { get; set; }

    }

}
