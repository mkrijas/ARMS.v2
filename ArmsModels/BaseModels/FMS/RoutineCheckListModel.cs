using ArmsModels.BaseModels.General;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.FMS
{
    public class RoutineCheckListModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RoutineCheckListModel>(Json);
        }
        public int? RoutineCheckListID { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public string TruckName { get; set; }
        public TruckModel Truck { get; set; }
        public string SelectListNames { get; set; }
        public string ItemIDs { get; set; }
        public string ItemNames { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public List<TruckModel> TruckList { get; set; }
        public List<IntStringValuesModel> CheckedItemLists { get; set; }
        public RoutineCheckListMasterModel RoutineCheckItem { get; set; }
        public List<RoutineCheckListMasterModel> RoutineCheckListItem { get; set; }
    }
}