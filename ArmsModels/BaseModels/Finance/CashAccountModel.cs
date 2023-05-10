using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class CashAccountModel
    {
        public int? CashAccountID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(8)]
        public string CashCode { get; set; }
        [Required]
        public ChartOfAccountModel Coa { get; set; } = new();
      
        public int? BranchID { get; set; }
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }
        public bool? IsDisabled { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}
