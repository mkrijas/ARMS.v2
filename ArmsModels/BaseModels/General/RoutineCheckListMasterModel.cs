using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.General
{
    // Model representing a master checklist item for routine checks
    public class RoutineCheckListMasterModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RoutineCheckListMasterModel>(Json);
        }
        public int? ItemID { get; set; } // Unique identifier for the checklist item
        public int? BranchID { get; set; }
        [Required]
        public string ItemName { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsExpired { get; set; } = false;
        public int? ValidDays { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CurrentTruckLastUpdatedDate { get; set; }
        public string Description { get; set; }
        public string DetailDescription { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    // Model representing a pair of integer and string values
    public class IntStringValuesModel 
    {
        public int? IntVal { get; set; }
        public string StringVal { get; set; }
    }
}