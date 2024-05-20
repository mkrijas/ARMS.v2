using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;

namespace Core.BaseModels.Operations
{
    public class TruckAvailabilityModel
    {
        public int? TruckAvailabilityID { get; set; }
        public int? TrucKID { get; set; }
        public int? BranchID { get; set; }
    }
    public class RequestApprovalHistoryModel : ICloneable
    {
        public int? RequestApprovalHistoryID { get; set; }
        public int? TruckID { get; set; }
        public int? DriverID { get; set; }
        public TruckModel Truck { get; set; }
        public int? RequestedBranchID { get; set; }
        public BranchModel RequestedBranch { get; set; }
        public DateTime? RequestedDate { get; set; }
        public UserInfoModel RequestedUserInfo { get; set; } = new();
        public int? RespondedBranchID { get; set; }
        public int? OpeningKM { get; set; }
        public int? ClosingKM { get; set; }
        public BranchModel RespondedBranch { get; set; }
        public DateTime? RespondedDate { get; set; }
        public UserInfoModel RespondedUserInfo { get; set; } = new();
        public byte? RequestStatus { get; set; }
        public decimal? Fuel { get; set; }
        public decimal? Expenses { get; set; }
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
