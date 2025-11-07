using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a sundry payment transaction
    public class SundryPaymentModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SundryPaymentModel>(Json);
        }
        public int? SundryPaymentID { get; set; } // Unique identifier for the sundry payment

        public bool deferredExpenditure { get; set; } = false;
        [RequiredIfTrue(nameof(deferredExpenditure))]
        public DateTime? beginDate { get; set; }
        [RequiredIfTrue(nameof(deferredExpenditure))]
        public DateTime? EndDate { get; set; }
        public string Reference { get; set; }
        [ValidateComplexType]
        [MustContain(ErrorMessage = "No particulars added for payment!")]
        public List<SundryPaymentEntryModel> Entries { get; set; } = new(); // List of entries associated with the sundry payment
        [Required]
        public string PaymentMode { get; set; }
        [Required]
        public string PaymentArdCode { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public string PayeeName { get; set; }
        [Required]
        public string PayeeContactNo { get; set; }
        public virtual int? JobcardID { get; set; }
    }

    // Model representing an entry in a sundry payment transaction
    public class SundryPaymentEntryModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SundryPaymentEntryModel>(Json);
        }
        public long? ID { get; set; } // Unique identifier for the sundry payment entry
        public int? ParentID { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public string SubArdCode { get; set; }
        public virtual string UsageCodeDescription { get; set; }
        [Required]
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string Reference { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
    }
}