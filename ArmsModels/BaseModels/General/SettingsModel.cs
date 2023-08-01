using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace ArmsModels.BaseModels.General
{
    public class SettingsModel
    {
        public int? SettingsID { get; set; }
        public int? BranchID { get; set; }
        public string SettingsName { get; set; }
        public string SettingsDescription { get; set; }
        public bool? RecordStatus { get; set; }
        public List<int?> RecordStatusList { get; set; } = new();
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
    }
}