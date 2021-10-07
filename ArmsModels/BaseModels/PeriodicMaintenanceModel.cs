using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;


namespace ArmsModels.BaseModels
{
    public class PeriodicMaintenanceModel
    {
        public int? PMID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        [Required]
        public int? TruckID { get; set; }
        [Required]
        public string TargetType { get; set; }
        public DateTime? TargetDate { get; set; }
        public long? TargetKM { get; set; }
        public bool AlertStatus { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}

