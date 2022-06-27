using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class BankAccountModel
    {
        public BankAccountModel()
        {
            
            UserInfo = new SharedModels.UserInfoModel();
            
        }
        public int? BankAccountID { get; set; }        
        [Required]
        [StringLength(maximumLength: 200)]
        public string BeneficiaryName { get; set; }
        [Required]
        [StringLength(maximumLength: 16)]
        public string AccountNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 11)]
        public string IfscCode { get; set; }      
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
