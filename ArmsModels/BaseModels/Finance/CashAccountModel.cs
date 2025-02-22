using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a cash account
    public class CashAccountModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CashAccountModel>(Json);
        }
        public int? CashAccountID { get; set; } // Unique identifier for the cash account
        [Required]
        public string Title { get; set; }
        public string CashCode { get; set; }
        [Required]
        public ChartOfAccountModel Coa { get; set; } = new(); // Chart of Accounts associated with the cash account
        public int? BranchID { get; set; }
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }
        public string AccountType { get; set; } // CASH/Wallet/Purse etc
        public bool? IsDisabled { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}