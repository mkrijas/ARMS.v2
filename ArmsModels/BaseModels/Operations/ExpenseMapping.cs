using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ExpenseMapping
    {
        public int? ExpenseID { get; set; }
        [Required]
        public string ExpenseTitle { get; set; }        
        public virtual string ExpenseCode { get; set; }
        [Required]
        public int? MappedCoaID { get; set; }
        public  virtual string AccountName { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();

    }
}
