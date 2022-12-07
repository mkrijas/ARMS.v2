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
        [StringLength(9, MinimumLength = 9)]
        public string MicrCode { get; set; }
        [StringLength(8, MinimumLength = 8)]
        public string SwiftCode { get; set; }
        public string BankTitle { get; set; }
        public string BankBranch { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class OwnBankModel
    {
        public OwnBankModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            _userInfo = new SharedModels.UserInfoModel();
        }
        SharedModels.UserInfoModel _userInfo;
        public int? ID { get; set; }
        public BankPostingGroupModel PostingGroup { get; set; } = new BankPostingGroupModel();
        public BankAccountModel BankAccountInfo { get; set; } = new BankAccountModel();
        public AddressModel AddressInfo { get; set; } = new AddressModel();
        public ContactModel ContactInfo { get; set; } = new ContactModel();
        public int? BranchID { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Must have 8 characters")]
        public string BankCode { get; set; }
        public bool IsGstRegistered { get; set; } = false;

        [StringLength(15, MinimumLength = 15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
        public SharedModels.UserInfoModel UserInfo
        {
            get
            {
                return _userInfo;
            }
            set
            {
                _userInfo = value;
                BankAccountInfo.UserInfo = _userInfo;
                AddressInfo.UserInfo = _userInfo;
                ContactInfo.UserInfo = _userInfo;
            }
        }
    }

    public class BankPostingGroupModel
    {
        public BankPostingGroupModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ID { get; set; }
        public string Title { get; set; }
        public ChartOfAccountModel BankAccount { get; set; }
        public ChartOfAccountModel BankCharges { get; set; }
        public ChartOfAccountModel ProcessingFee { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class UnReconciledBankEntryModel
    {
        public int? ID { get; set; }
        [Required]
        public int? Nature { get; set; } //SELECT  -1 as Payment,1 as Receipt
        [Required]
        public string NatureName { get; set; } //SELECT  -1 as Payment,1 as Receipt
        [Required]
        public DateTime? TransactionDate { get; set; }
        [Required]
        public string ArdCode { get; set; }  // Bank Code from Bank Model
        [Required]
        public int BankID { get; set; }
        public string InstrumentType { get; set; }// Cheuque,DD,E Transfer
        [Required]
        public DateTime? InstrumentDate { get; set; }
        [Required]
        public string InstrumentReference { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string PaymentRemarks { get; set; }
        public bool IsReconciled { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
        public virtual ReconciledBankEntryModel ReconciledInfo { get; set; } = new();

    }

    public class ReconciledBankEntryModel
    {
        public int? ID { get; set; }
        public DateTime? ReconciledDate { get; set; }
        public string Remarks { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}
