using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class PaymentMemoModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PaymentMemoModel>(Json);
        }
        public int? PaymentMemoID { get; set; }
        [Required]
        public string BusinessNature { get; set; } // "Suppier,Customer,Renter"
        [Required]
        public PartyModel PartyInfo { get; set; }
        public int? PartyCoaID { get; set; }
        [Required]
        public string Reference { get; set; }
        public decimal? BankCharges { get; set; }      
        [Required]
        public byte? PaymentStatus { get; set; } = 0; // 0 - generated; 1 - initiated; 2 - completed;
        public List<BillsPaidModel> Bills { get; set; } = new();
    }

    public class PaymentMemoPrintDetailModel
    {
        public int? PaymentMemoID { get; set; }
        public string DocNumber { get; set; }
        public string PartyName { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankAccount { get; set; }
        public string IfscCode { get; set; }
        public string Reference { get; set; }
        public decimal BankCharges { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DueOn { get; set; }
    }

    public class BillsPaidModel
    {
        decimal? _PayAmount;
        public int? BpID { get; set; }
        public int? MID { get; set; }
        public bool IsMemo { get; set; } = false;
        public int? CoaID { get; set; }
        public string SubArdCode { get; set; }
        public virtual decimal? OutstandingAmount { get; set; }
        public virtual string OutstandingAmountDisplayText { get { return Math.Abs(OutstandingAmount ?? 0).ToString() + " " + ((OutstandingAmount ?? 0) < 0 ? "Cr" : "Dr"); } }
        public decimal? PayAmount
        {
            get { return _PayAmount; }
            set
            {
                _PayAmount = value;
            }
        }
        public virtual string BranchName { get; set; }
        public virtual int? BranchID { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
    }

    public class PaymentInitiatedModel : ICloneable
    {
        public int? PaymentInitiatedID { get; set; }
        [Required]
        public DateTime? DueOn { get; set; }
        [Required]
        public DateTime? InitiatedDocumentDate { get; set; } = DateTime.Today;
        public string DocumentNumber { get; set; }
        public int? BranchID { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<PaymentMemoModel> SelectedMemos { get; set; } = new();
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PaymentInitiatedModel>(Json);
        }
    }

    public class PaymentFinishModel : TransactionBaseModel, ICloneable
    {
        public int? PaymentFinalizeID { get; set; }
        [Required]
        public string PaymentMode { get; set; }  // Bank,Cash   
        public string PaymentTool { get; set; }  // Cheque,DD
        [Required]
        public string PaymentArdCode { get; set; }
        //public decimal? BankCharges { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; } = DateTime.Today;
        public int? PaymentInitiatedID { get; set; }
        public DateTime? DueOn { get; set; }
        public DateTime? InitiatedDocumentDate { get; set; } = DateTime.Today;
        public List<PaymentMemoModel> SelectedMemos { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PaymentFinishModel>(Json);
        }

        public PaymentFinishModel()
        {
        }
    }
}