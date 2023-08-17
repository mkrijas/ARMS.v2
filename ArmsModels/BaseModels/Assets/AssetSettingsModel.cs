using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class AssetSettingsModel
    {
        public int? CheckListID { get; set; }
        public int? AssetTransferID { get; set; }
        public int? SettingsID { get; set; }
        public virtual int? SubClassID { get; set; }
        public virtual string SettingsName { get; set; }
        [Required]
        public string SettingsDescription { get; set; }
        public virtual bool RecordStatus { get; set; }
        public virtual bool? IsRecieved { get; set; } = false;
    }
}
