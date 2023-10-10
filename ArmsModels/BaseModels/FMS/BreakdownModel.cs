using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class BreakdownModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CashAccountModel>(Json);
        }
        public BreakdownModel()
        {
            UserInfo = new();
        }
        public int? BreakdownID { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public string BreakdownType { get; set; }
        [Required]
        public DateTime? BreakdownTime { get; set; } = DateTime.Now;
        [Required]
        public int? TruckID { get; set; }
        public bool IsClaimInitiated { get; set; }
        public virtual string RegNo { get; set; }
        [Required]
        public string Detail { get; set; }
        public string ContactNumber { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}