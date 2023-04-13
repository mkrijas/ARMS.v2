using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ArmsModels.BaseModels
{
    public class SundryPaymentModel:TransactionBaseModel
    {
        public int? SundryPaymentID { get; set; }        
        public bool  deferredExpenditure { get; set; } = false;
        [RequiredIfTrue(nameof(deferredExpenditure))]
        public DateTime ? beginDate { get; set; }
        [RequiredIfTrue(nameof(deferredExpenditure))]
        public DateTime ? EndDate { get; set; }
        
        public string Reference { get; set; }
        
        [ValidateComplexType]
        [MustContain(ErrorMessage = "No particulars added for payment!")]
        public List<SundryPaymentEntryModel> Entries { get; set; } = new();
        [Required]
        public string PaymentMode { get; set; }        
        [Required]
        public string PaymentArdCode { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode","Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public string PayeeName { get; set; }
        [Required]
        public string PayeeContactNo { get; set; }
    }

    public class SundryPaymentEntryModel
    {
        public long? ID { get; set; }
        public int? ParentID { get; set; }
        [Required]
        public int? BranchID { get; set; }        
        [Required]
        public string UsageCode { get; set; }
        [Required]
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string Reference { get; set; }
    }



}
