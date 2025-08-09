using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a tax purchase transaction
    public class CSGrnModel : TransactionBaseModel, IValidatableObject, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CSGrnModel>(Json);
        }
        public CSGrnModel()
        {
            NatureOfTransaction = "Centralized Store Grn";
        }
        public int? GrnID { get; set; } // Unique identifier for the tax purchase
        [Required]
        public PartyModel PartyInfo { get; set; }
        [Required]
        public bool IsCredit { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        [ValidateComplexType]
        public List<CSGrnItemModel> Items { get; set; } = new(); // List of items associated with the purchase
        public int? POID { get; set; }
        public decimal? IssuedQty { get; set; }
        public int? NoOfGR { get; set; }
        public decimal? RoundOff { get; set; }
        public string Reference { get; set; } // Additional remarks for the transaction
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
            if (Items.Count == 0)
                yield return new ValidationResult("No Items selected!");
            else if (InvoiceDate.HasValue && DocumentDate.HasValue && InvoiceDate.Value > DocumentDate.Value)
                yield return new ValidationResult("Invoice Date must be on or before document date!");
        }
    }

    public class CSGrnItemModel
    {
        decimal? _qty, _rate; // Backing fields for quantity and rate
        public long? GrnItemID { get; set; } // Unique identifier for the tax purchase item
        public int? GrnID { get; set; }
        public long? POItemID { get; set; } // Unique identifier for the tax purchase item
        [Required]
        public int? ItemID { get; set; }
        [Required]
        public string UoM { get; set; } // Unit of Measure for the item
        public int? CoaID { get; set; }
        [Required]
        public decimal? ItemQty { get { return _qty; } set { _qty = value; Amount = Math.Round((_qty * value) ?? 0, 2); } }
        [Required]
        public decimal? MRP { get; set; }
        [Required]
        public decimal? ItemRate { get { return _rate; } set { _rate = value; Amount = Math.Round((_qty * value) ?? 0, 2); } }
        [Required]
        public decimal? GstRate { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public decimal? ItemGstVal { get; set; } = 0;
        public decimal? Amount { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual string ItemGroupDescription { get; set; }
        public virtual string PartNumber { get; set; }
        public virtual decimal? BaseQty { get; set; }
        public virtual decimal? BaseRate { get; set; }
        public virtual string BatchNo { get; set; }
    }
}