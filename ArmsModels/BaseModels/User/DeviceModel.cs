using Microsoft.VisualBasic;
using System;

namespace Core.BaseModels.User
{
    public class DeviceModel
    {
        public int? ID { get; set; }
        public string UserID { get; set; }
        public string DeviceID { get; set; }
        public DateTime? TimeStamp { get; set; }
        public byte? RecordStatus { set; get; }
    }
}