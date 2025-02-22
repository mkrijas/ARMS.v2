using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a sundry receipt transaction
    public class SundryReceiptModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SundryReceiptModel>(Json);
        }
        public int? SundryReceiptID { get; set; } // Unique identifier for the sundry receipt
        [Required]
        public string ReceiptMode { get; set; }
        [Required]
        public string ReceiptArdCode { get; set; }
        [Required]
        public int? ReceiptCoaID { get; set; }
        public string Reference { get; set; }
        [Required]
        public string PayerName { get; set; }
        [Required]
        public string PayerContactNo { get; set; }
        [Required]
        [ValidateComplexType]
        [MustContain(ErrorMessage = "No particulars selected for Receipt!")]
        public List<SundryReceiptEntryModel> Entries { get; set; } = new(); // List of entries associated with the sundry receipt
    }

    // Model representing an entry in a sundry receipt transaction
    public class SundryReceiptEntryModel
    {
        public long? ID { get; set; }
        public int? ParentID { get; set; }
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