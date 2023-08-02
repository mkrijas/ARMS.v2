using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;

namespace ArmsModels.BaseModels
{
    public class AssetSettingsModel
    {
        public int? SettingsID { get; set; }
        public int? SubClassID { get; set; }
        public string SettingsName { get; set; }
        public string SettingsDescription { get; set; }
        public bool RecordStatus { get; set; }
        public List<int?> RecordStatusList { get; set; } = new();
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
