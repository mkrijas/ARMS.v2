using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class TaxPurchaseModel : TransactionBaseModel, IValidatableObject, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TaxPurchaseModel>(Json);
        }
        public TaxPurchaseModel()
        {
            NatureOfTransaction = "TaxPurchase";
        }
        public int? PID { get; set; }
        //public int? SundryPaymentID { get; set; }
        public bool deferredExpenditure { get; set; } = false;
        [RequiredIfTrue(nameof(deferredExpenditure))]
        public DateTime? beginDate { get; set; }
        [RequiredIfTrue(nameof(deferredExpenditure))]
        public DateTime? EndDate { get; set; }
        public int? GRNID { get; set; }
        [Required]
        public PartyModel PartyInfo { get; set; }
        [Required]
        public bool IsCredit { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        public decimal? AdditionalTDS { get; set; }
        [Required]
        public bool NonStoreInventory { get; set; }
        [ValidateComplexType]
        public List<TaxPurchaseExpenseModel> Expenses { get; set; } = new();
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Expenses.Count == 0 && Items.Count == 0)
                yield return new ValidationResult("No Items or Expenses selected!");
        }
    }

    public class TaxPurchaseExpenseModel
    {
        public long? TpeID { get; set; }
        public int? PID { get; set; }
        [Required]
        public decimal? GstRate { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public virtual string UsageCodeDescription { get; set; }
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public string BillReference { get; set; }
    }

    public class TaxPurchaseItemModel
    {
        decimal? _qty, _rate;
        public long? TpiID { get; set; }
        public int? PID { get; set; }
        public decimal? GstRate { get; set; }
        [Required]
        public int? ItemID { get; set; }
        [Required]
        public decimal? ItemRate { get { return _rate; } set { _rate = value; Amount = (_qty * value); } }
        [Required]
        public decimal? ItemQty { get { return _qty; } set { _qty = value; Amount = (_rate * value); } }
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public virtual string ItemDescription { get; set; }
    }
}