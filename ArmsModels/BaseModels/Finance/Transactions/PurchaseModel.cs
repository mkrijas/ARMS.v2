using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a tax purchase transaction
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
        public int? PID { get; set; } // Unique identifier for the tax purchase
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
        public string TaxPurchaseType { get; set; }
        [ValidateComplexType]
        public List<TaxPurchaseExpenseModel> Expenses { get; set; } = new(); // List of expenses associated with the purchase
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new(); // List of items associated with the purchase
        [ValidateComplexType]
        public List<AssetPOModel> Assets { get; set; } = new(); // List of assets associated with the purchase
        public decimal? TDS { get; set; }

        // Method to validate the model
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var baseErrors = base.Validate(validationContext);
            if (baseErrors != null && baseErrors.Any())
            {
                foreach (var item in baseErrors)
                {
                    yield return item;
                }
            }
            if (Expenses.Count == 0 && Items.Count == 0 && Assets.Count == 0)
                yield return new ValidationResult("No Items or Expenses selected!");
            else if (InvoiceDate.HasValue && DocumentDate.HasValue && InvoiceDate.Value > DocumentDate.Value)
                yield return new ValidationResult("Invoice Date must be on or before document date!");
        }
    }

    // Model representing an expense associated with a tax purchase
    public class TaxPurchaseExpenseModel
    {
        public long? TpeID { get; set; } // Unique identifier for the tax purchase expense
        public int? PID { get; set; }
        [Required]
        public decimal? GstRate { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public string SubArdCode { get; set; }
        public virtual string UsageCodeDescription { get; set; }
        public virtual string GstMechanism { get; set; }
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public string BillReference { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
    }

    public class TaxPurchaseItemModel
    {
        decimal? _qty, _rate; // Backing fields for quantity and rate
        public long? TpiID { get; set; } // Unique identifier for the tax purchase item
        public int? PID { get; set; }
        public decimal? GstRate { get; set; }
        [Required]
        public int? ItemID { get; set; }
        [Required]
        public decimal? ItemRate { get { return _rate; } set { _rate = value; Amount = Math.Round((_qty * value) ?? 0, 2); } }
        [Required]
        public decimal? ItemQty { get { return _qty; } set { _qty = value; Amount = Math.Round((_qty * value) ?? 0, 2); } }
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public virtual string ItemCode { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual string ItemGroupDescription { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
        public virtual decimal? OriginalQty { get; set; }
    }
}