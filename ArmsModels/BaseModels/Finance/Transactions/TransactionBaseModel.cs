using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TransactionBaseModel
    {
        public TransactionBaseModel()
        {
            UserInfo = new();
        }
        public int? MID { get; set; }
        [Required]
        public DateTime? DocumentDate { get; set; } = DateTime.Today;
        public string DocumentNumber { get; set; } = "New";
        [Required]
        public string NatureOfTransaction { get; set; } //Deposit,Purchase,Payment,Receipt,Prepayment,BankCharges,Main,Depreciation,CWIP,Capitalization
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public virtual decimal? TotalAmount { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        [Required]
        public string Narration { get; set; }
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }
        public bool IsInterBranch { get; set; } = false;
        public int? InterBranchTranID { get; set; } = null;
        [RequiredIfTrue("IsInterBranch")]
        public int? OtherBranchID { get; set; }
        public string OtherBranchName { get; set; }

        public string FileName { get; set; }
        //public virtual List<FileNames> filenames { get; set; } = new List<FileNames>();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
    //public class FileNames
    //{
    //    public string FileName { get; set; }
    //}


    public class AccountInfoViewModel
    {
        public DateTime? DocumentDate { get; set; }
        public string DocumentNumber { get; set; }      
        public string CostCenter { get; set; }
        public string Dimension { get; set; }
        public string Narration { get; set; }
        public List<AccountInfoViewSubModel> Entries { get; set; }  = new List<AccountInfoViewSubModel>();
    }



    public class AccountInfoViewSubModel
    {        
        public string AccountName { get; set; }
        public string BranchName { get; set; }
        public decimal? Amount {  get; set; }
        public string drcr { get { return Amount != null && Amount < 0 ? "Cr" : "Dr"; } }
        public string Reference { get; set; }
    }

    public class GstModel
    {        
        public virtual decimal? GstRate { get; set; }
        public decimal? CGST { get; set; } = 0;
        public decimal? SGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;

        public virtual decimal? TotalGst { get { return (CGST??0) + (SGST??0) + (IGST??0);  }  }
    }

    public class ChequeModel
    {
        public int? ID { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? BankID { get; set; }
    }    
      

    }
