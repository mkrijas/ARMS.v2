using ArmsModels.SharedModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.General
{
    public class RoutineCheckListMasterModel
    {
        public int? ItemID { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public string ItemName { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public int? ValidDays { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CurrentTruckLastUpdatedDate { get; set; }
        public string Description { get; set; }
        public UserInfoModel UserInfo { get; set; }

    }
}
