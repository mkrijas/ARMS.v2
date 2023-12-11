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
    public class RequestApprovalHistoryModel :ICloneable
    {
        public int? RequestApprovalHistoryID { get; set; }
        public int? TruckID { get; set; }
        public TruckModel Truck { get; set; }
        public int? RequestedBranchID { get; set; }
        public BranchModel RequestedBranch { get; set; }
        public DateTime? RequestedDate { get; set; }
        public UserInfoModel RequestedUserInfo { get; set; } = new();
        public int? RespondedBranchID { get; set; }
        public BranchModel RespondedBranch { get; set; }
        public DateTime? RespondedDate { get; set; }
        public UserInfoModel RespondedUserInfo { get; set; } = new();
        public bool? IsApproved { get; set; }
        public string ApprovedStatus { get; set; }

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RequestApprovalHistoryModel>(Json);
        }
    }
}
