using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Operations
{
    // Represents the availability status of a truck
    public class TruckAvailabilityModel
    {
        public int? TruckAvailabilityID { get; set; } // Unique identifier for the truck availability record (nullable)
        public int? TrucKID { get; set; }
        public int? BranchID { get; set; }
    }
    
    // Represents the history of request approvals for truck usage
    public class RequestApprovalHistoryModel : ICloneable
    {
        public int? RequestApprovalHistoryID { get; set; } // Unique identifier for the request approval history record (nullable)
        public string DocNumber {  get; set; }
        public int? TruckID { get; set; }
        public int? DriverID { get; set; }
        public TruckModel Truck { get; set; } // Truck information associated with the request
        public int? RequestedBranchID { get; set; }
        public BranchModel RequestedBranch { get; set; }
        public DateTime? RequestedDate { get; set; }
        public UserInfoModel RequestedUserInfo { get; set; } = new();
        public int? RespondedBranchID { get; set; }
        [Required]
        public int? OpeningKM { get; set; }
        [Required]
        public int? ClosingKM { get; set; }
        public BranchModel RespondedBranch { get; set; }
        public DateTime? RespondedDate { get; set; }
        public UserInfoModel RespondedUserInfo { get; set; } = new();
        public byte? RequestStatus { get; set; }
        public decimal? Fuel { get; set; }
        public decimal? Expenses { get; set; }
        public List<string> Uploads { get; set; }
        public string Remarks { get; set; }
        public List<AssetSettingsModel> CheckList { get; set; } // List of asset settings associated with the transfer
        // Property to get the status text based on the request status
        public string StatusText
        {
            get
            {
                switch (RequestStatus)
                {
                    case 0:
                        return "Pending";
                    case 1:
                        return "Approved";
                    case 2:
                        return "Completed";
                    case 99:
                        return "Rejected";
                    default:
                        return null;
                }
            }
        }

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RequestApprovalHistoryModel>(Json);
        }
    }
}
