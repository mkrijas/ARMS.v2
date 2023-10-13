using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class PeriodicMaintenanceInitiateModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PeriodicMaintenanceInitiateModel>(Json);
        }
        public PeriodicMaintenanceInitiateModel()
        {
            UserInfo = new();
        }
        public int? PMIID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Remarks { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public string RegNo { get; set; }
        [Required]
        public DateTime? NDate { get; set; } // Date or Audometer whichever comes first insitiate Notification
        [Required]
        public long? NAudometer { get; set; }
        //Get when Notification is created
        public int? NotificationID { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public int? JobCardID { get; set; } = null;
    }

    public class PeriodicMaintenanceConcludeModel
    {
        public PeriodicMaintenanceConcludeModel()
        {
            UserInfo = new();
            Initiator = new();
        }
        public int? PMCID { get; set; }
        [Required]
        public int? PMIID { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public long? Audometer { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public virtual PeriodicMaintenanceInitiateModel Initiator { get; set; }
    }
}