using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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
        [StringLength(18, ErrorMessage = "Account number should between 9 to 18 digit", MinimumLength = 9)]        
        public string AccountNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "IFSC Code must be a string with a length of 11")]
        public string IfscCode { get; set; }
        [StringLength(9, MinimumLength = 9)]
        public string MicrCode { get; set; }
        [StringLength(8, MinimumLength = 8)]
        public string SwiftCode { get; set; }
        [Required]
        public string BankTitle { get; set; }
        public string BankBranch { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } 
    }

    public class OwnBankModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<OwnBankModel>(Json);
        }
        public OwnBankModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            _userInfo = new SharedModels.UserInfoModel();
        }
        SharedModels.UserInfoModel _userInfo;
        public int? ID { get; set; }
        [Required]
        public BankPostingGroupModel PostingGroup { get; set; }
        [Required]
        [ValidateComplexType]
        public BankAccountModel BankAccountInfo { get; set; } = new BankAccountModel();
        [ValidateComplexType]
        public AddressModel AddressInfo { get; set; } = new AddressModel();
        [ValidateComplexType]
        public ContactModel ContactInfo { get; set; } = new ContactModel();
        [Required]
        public int? BranchID { get; set; }
        public string BankCode { get; set; }
        public bool IsGstRegistered { get; set; } = false;
        [RequiredIfTrue("IsGstRegistered")]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
        public string TANNo { get; set; }
        public SharedModels.UserInfoModel UserInfo
        {
            get
            { return _userInfo; }
            set
            {
                _userInfo = value;
                BankAccountInfo.UserInfo = _userInfo;
                AddressInfo.UserInfo = _userInfo;
                ContactInfo.UserInfo = _userInfo;
            }
        }
    }

    public class BankPostingGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<BankPostingGroupModel>(Json);
        }
        public BankPostingGroupModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ID { get; set; }
        [Required]
        public string Title { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel BankAccount { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel BankCharges { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel ProcessingFee { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class UnReconciledBankEntryModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<UnReconciledBankEntryModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public int? Nature { get; set; } //SELECT  -1 as Payment,1 as Receipt
        [Required]
        public string NatureName { get { return Nature == 1 ? "Receipt" : "Payment"; } }
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
        [Required]
        public DateTime? ReconciledDate { get; set; }
        [Required]
        public int? BankID { get; set; }
        [Required]
        public bool? IsExisting { get; set; }
        [Required]
        public long? AccountEntryID { get; set; }
        public DateTime? DocDate { get; set; }
        public string AccountName { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public virtual string DocNumber { get; set; }
        public virtual string Narration { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    public class ReconciledBankSummaryModel
    {
        public string BankOrCompany { get; set; }
        public decimal? OpeningAmount { get; set; }
        public decimal? TransactionAmount { get; set; }
        public decimal? ClossingAmount { get; set; }
    }
}