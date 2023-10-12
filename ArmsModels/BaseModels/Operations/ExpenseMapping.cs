using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ExpenseMapping : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ExpenseMapping>(Json);
        }
        public int? ExpenseID { get; set; }
        [Required]
        public string ExpenseTitle { get; set; }
        public virtual string ExpenseCode { get; set; }
        public string Area { get; set; }// Operation,Maintenance 
        [Required]
        public ChartOfAccountModel MappedCoaID { get; set; } = new();
        public virtual string AccountName { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();

    }
}
