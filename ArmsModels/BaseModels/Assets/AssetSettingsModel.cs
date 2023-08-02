using ArmsModels.SharedModels;
using System;

namespace ArmsModels.BaseModels
{
    public class AssetSettingsModel
    {
        public int? SettingsID { get; set; }
        public int? SubClassID { get; set; }
        public string SettingsName { get; set; }
        //public bool IsChecked { get; set; }
        public string SettingsDescription { get; set; }
        public bool RecordStatus { get; set; }
    }
}
