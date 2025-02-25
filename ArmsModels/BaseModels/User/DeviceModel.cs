using Microsoft.VisualBasic;
using System;

namespace Core.BaseModels.User
{
    // Represents a device associated with a user
    public class DeviceModel
    {
        public int? ID { get; set; } // Unique identifier for the device record (nullable)
        public string UserID { get; set; }
        public string DeviceID { get; set; }
        public DateTime? TimeStamp { get; set; }
        public byte? RecordStatus { set; get; }
    }
}