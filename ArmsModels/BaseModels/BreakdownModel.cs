using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class ReportBreakdownModel
    {
        public int? BreakdownID { get; set; }
        [Required]
        public string BreakdownType { get; set; }
        [Required]
        public int? TruckID { get; set; }
        [Required]
        public string Detail { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
