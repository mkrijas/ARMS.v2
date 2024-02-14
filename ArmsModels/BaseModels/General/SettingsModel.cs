using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;
using System.Linq;

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

    public class GeneralSettingsModel
    {
        public string SettingId { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
        public string? ValueOptions { get; set; }
        public List<string>? ValueOptionsList => !string.IsNullOrEmpty(ValueOptions) ? ValueOptions.Split(',').ToList() : null;
        public bool? RecordStatus { get; set; }
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public KeyValuePair<string, string> selectedValue { get; set; }
        public List<ValueOptions> Values { get; set; }
        public string ValueSelectType { get; set; }
        public bool HideTextField { get; set; }
        public string KeyString { get; set; }
    }
    public class ValueOptions
    {
        public string? id { get; set; }
        public string val { get; set; }
    }
}