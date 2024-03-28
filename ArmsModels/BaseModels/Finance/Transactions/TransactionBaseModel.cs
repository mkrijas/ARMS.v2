using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class TransactionBaseModel: IValidatableObject
    {
        public TransactionBaseModel()
        {
            UserInfo = new();
        }
        public int? MID { get; set; }
        public int? ReverseMID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DocumentDate { get; set; } = DateTime.Today;
        public string DocumentNumber { get; set; } = "New";
        [Required]
        public string NatureOfTransaction { get; set; } //Deposit,Purchase,Payment,Receipt,Prepayment,BankCharges,Main,Depreciation,CWIP,Capitalization
        [Required]
        public int? BranchID { get; set; }
        public virtual decimal? TotalTaxableAmount { get; set; }
        public string FileName { get; set; }
        [RequiredIf("FileName", "null")]
        public virtual decimal? TotalAmount { get; set; }        
        [Required]
        public string Narration { get; set; }
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }
        public bool IsInterBranch { get; set; } = false;
        public int? InterBranchTranID { get; set; } = null;
        [RequiredIfTrue("IsInterBranch")]
        public int? OtherBranchID { get; set; }
        public string OtherBranchName { get; set; }
        public string OtherBranch { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public bool IsDisabled { get; set; }
        public bool? IsApplicable { get; set; }
        public decimal? CashWithdrawal { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        { 
              if(DocumentDate.HasValue && DocumentDate.Value.Date > DateTime.Today)
            {
                yield return new ValidationResult("Document date cannot be a future date!");
            }
        }
        }

    public class AccountInfoViewModel
    {
        public DateTime? DocumentDate { get; set; } = new();
        public string DocumentNumber { get; set; }        
        public string Narration { get; set; }
        public List<AccountInfoViewSubModel> Entries { get; set; } = new List<AccountInfoViewSubModel>();
    }


    public class AccountInfoViewSubModel
    {
        public string AccountName { get; set; }
        public string BranchName { get; set; }       
        public decimal? Amount { get; set; }
        public string drcr { get { return Amount != null && Amount < 0 ? "Cr" : "Dr"; } }
        public string Reference { get; set; }
        public string CostCenter { get; set; }
        public string Dimension { get; set; }
    }
    public class GstModel : IValidatableObject, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GstModel>(Json);
        }
        public virtual decimal? GstRate { get; set; }
        [ValidateComplexType]
        public decimal? CGST { get; set; } = 0;
        [ValidateComplexType]
        public decimal? SGST { get; set; } = 0;
        [ValidateComplexType]
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public virtual decimal? TotalGst { get { return (CGST ?? 0) + (SGST ?? 0) + (IGST ?? 0); } }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IGST > 0 && (CGST > 0 || SGST > 0))
            {
                yield return new ValidationResult("You can enter either IGST or CGST AND SGST Values");
            }

            else if ((CGST > 0 || SGST > 0) && IGST > 0)
            {
                yield return new ValidationResult("You can enter either IGST or CGST AND SGST Values");
            }
            if(CGST.Value != SGST.Value)
            {
                yield return new ValidationResult("CGST AND SGST Values must be equal");
            }
        }
    }
    public class ChequeModel
    {
        public int? ID { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? BankID { get; set; }
    }
}