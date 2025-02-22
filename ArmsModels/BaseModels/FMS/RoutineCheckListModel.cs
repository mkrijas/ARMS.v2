using ArmsModels.BaseModels.General;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.FMS
{
    // Model representing a routine checklist for vehicles
    public class RoutineCheckListModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RoutineCheckListModel>(Json);
        }
        public int? RoutineCheckListID { get; set; } // Unique identifier for the routine checklist
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
        public string Remarks { get; set; }
        public int? ItemID { get; set; }
        public string Description { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public List<TruckModel> TruckList { get; set; } // List of trucks associated with the checklist
        public List<IntStringValuesModel> CheckedItemLists { get; set; } // List of checked items in the checklist
        public RoutineCheckListMasterModel RoutineCheckItem { get; set; } // Master item for the routine checklist
        public List<RoutineCheckListMasterModel> RoutineCheckListItem { get; set; } // List of items in the routine checklist
    }
}