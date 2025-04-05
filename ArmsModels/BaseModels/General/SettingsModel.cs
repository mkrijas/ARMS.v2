using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;
using System.Linq;

namespace ArmsModels.BaseModels.General
{
    // Model representing application settings
    public class SettingsModel
    {
        public int? SettingsID { get; set; }
        public int? BranchID { get; set; }
        public string SettingsName { get; set; }
        public string SettingsDescription { get; set; }
        public bool? RecordStatus { get; set; }
        public bool? ValueInput { get; set; }
        public List<int?> RecordStatusList { get; set; } = new();
        public string Value { get; set; } // <-- Add this
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing general settings
    public class GeneralSettingsModel
    {
        public string SettingId { get; set; } // Identifier for the setting
        public string SettingName { get; set; }
        public string Value { get; set; }
        public string? ValueOptions { get; set; }
        public List<string>? ValueOptionsList => !string.IsNullOrEmpty(ValueOptions) ? ValueOptions.Split(',').ToList() : null; // List of value options split from the ValueOptions string
        public bool? RecordStatus { get; set; }
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public KeyValuePair<string, string> selectedValue { get; set; } // Selected value as a key-value pair
        public List<ValueOptions> Values { get; set; } // List of value options for the setting
        public string ValueSelectType { get; set; }
        public bool HideTextField { get; set; }
        public string KeyString { get; set; }
    }

    // Model representing individual value options
    public class ValueOptions
    {
        public string? id { get; set; }
        public string val { get; set; }
    }
}