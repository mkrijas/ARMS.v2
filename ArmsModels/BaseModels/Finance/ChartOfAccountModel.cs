using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class ChartOfAccountModel
    {
        public ChartOfAccountModel()
        {
            UserInfo = new();
        }

        public int? CoaID { get; set; }
        public int? ParentID { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public string AccountType { get; set; }
        public bool SummaryAccount { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } 
    }
}
