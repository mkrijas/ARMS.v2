using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class AssetSettingsModel
    {
        public int? SettingsID { get; set; }
        public int? SubClassID { get; set; }
        public string SettingsName { get; set; }
        [Required]
        public string SettingsDescription { get; set; }
        public bool RecordStatus { get; set; }
        public int? CheckListID { get; set; }
        public bool? IsRecieved { get; set; }
    }
}
