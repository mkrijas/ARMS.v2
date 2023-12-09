using ArmsModels.SharedModels;
using System;

namespace Core.BaseModels.Operations
{
    public class TruckAvailabilityModel
    {
        public int? TruckAvailabilityID { get; set; }
        public int? TrucKID { get; set; }
        public int? BranchID { get; set; }
    }
    public class RequestApprovalHistoryModel
    {
        public int? RequestApprovalHistoryID { get; set; }
        public int? TrucKID { get; set; }
        public int? RequestedBranchID { get; set; }
        public DateTime? RequestedDate { get; set; }
        public UserInfoModel RequestedUserInfo { get; set; } = new();
        public int? RespondedBranchID { get; set; }
        public DateTime? RespondedDate { get; set; }
        public UserInfoModel RespondedUserInfo { get; set; } = new();
        public bool? IsApproved { get; set; }

    }
}
