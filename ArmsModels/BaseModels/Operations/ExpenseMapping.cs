using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents the mapping of expenses to a chart of accounts
    public class ExpenseMapping : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ExpenseMapping>(Json);
        }
        public int? ExpenseID { get; set; } // Unique identifier for the expense mapping (nullable)
        [Required]
        public string ExpenseTitle { get; set; }
        public virtual string ExpenseCode { get; set; }
        public string Area { get; set; }// Operation,Maintenance 
        [Required]
        public ChartOfAccountModel MappedCoaID { get; set; } = new(); // Required property for the mapped chart of accounts ID
        public virtual string AccountName { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();

    }
}
