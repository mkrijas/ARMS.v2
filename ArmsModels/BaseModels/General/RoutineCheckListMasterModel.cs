using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.General
{

    public class RoutineCheckListMasterModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RoutineCheckListMasterModel>(Json);
        }
        public int? ItemID { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public string ItemName { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsExpired { get; set; }
        public int? ValidDays { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CurrentTruckLastUpdatedDate { get; set; }
        public string Description { get; set; }
        public string DetailDescription { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
    public class IntStringValuesModel 
    {
        public int? IntVal { get; set; }
        public string StringVal { get; set; }
    }
}