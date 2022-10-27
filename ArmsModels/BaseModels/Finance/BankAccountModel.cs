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
        public string Bank { get; set; }
        public string BankBranch { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class OwnBankModel
    {
        public BankPostingGroupModel PostingGroup { get; set; } = new BankPostingGroupModel();
        public BankAccountModel BankAccountInfo { get; set; } = new BankAccountModel();
        public AddressModel AddressInfo { get; set; } = new AddressModel();
        public ContactModel ContactInfo { get; set; } = new ContactModel();
        public int? BranchID { get; set; }
        public bool IsGstRegistered { get; set; } = false;
        
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
    }

    public class BankPostingGroupModel
    {
        public int? ID { get; set; }
        public int? BankAccount { get; set; }
        public int? BankCharges { get; set; }
        public int? ProcessingFee { get; set; }
    }


}
