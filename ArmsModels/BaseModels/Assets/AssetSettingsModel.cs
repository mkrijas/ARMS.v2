using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace ArmsModels.BaseModels
{
    // Model representing asset settings
    public class AssetSettingsModel
    {
        public int? CheckListID { get; set; } // Unique identifier for the checklist associated with the asset settings
        public int? AssetTransferID { get; set; } // Unique identifier for the asset transfer
        public int? SettingsID { get; set; } // Unique identifier for the settings
        public string Value { get; set; }
        public string Condition { get; set; }
        public virtual int? SubClassID { get; set; }
        public virtual string SettingsName { get; set; }
        public virtual string SettingsDescription { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool? IsRecieved { get; set; } = false;
        public virtual bool? ValueInput { get; set; } = false;
        public virtual string Status
        {
            get
            {
                if ((bool)IsRecieved)
                {
                    return "Recieved";
                }
                else
                {
                    return "Not Recieved";
                }
            }
        }
    }
}
