using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.Finance.Transactions
{
    // Model representing a document voucher transaction
    public class DocumentVoucherModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DocumentVoucherModel>(Json);
        }
        public int? DocumentVoucherID { get; set; } // Unique identifier for the document voucher
        [Required]
        public AssetDocumentTypeModel DocumentType { get; set; } // Type of the document associated with the voucher
        //[Required]
        //public DateTime? FromDate { get; set; }
        //[Required]
        //public DateTime? ToDate { get; set; }
        //[Required]
        //public DateTime? InvoiceDate { get; set; }
        public bool IsAgent { get; set; }
        [RequiredIfTrue("IsAgent")]
        public PartyModel Agent { get; set; } // Information about the agent involved in the transaction
        [RequiredIf("IsAgent", " false")]
        public string PaymentMode { get; set; }
        [RequiredIf("IsAgent", " false")]
        public string PaymentArdCode { get; set; }
        [RequiredIf("IsAgent", " false")]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        public virtual decimal? GstRate { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public decimal? TotalTax { get; set; }
        public virtual string AccountName { get; set; }
        public List<DocumentVoucherSubModel> DocVoucherSubList { get; set; } = new(); // List of sub-models for document vouchers
    }

    // Model representing a sub-entry in a document voucher
    public class DocumentVoucherSubModel
    {
        public int? DocumentVoucherSubID { get; set; } // Unique identifier for the sub-entry
        public int? DocumntVoucherID { get; set; }
        public int? DocumentID { get; set; }
        public string SlipNo { get; set; }
        public string Reference { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public virtual bool? IsSelect { get; set; } = false;
        public virtual int? AssetID { get; set; }
        public virtual string AssetName { get; set; }
        public virtual string AssetCode { get; set; }
        //public virtual string CostCenterVal { get; set; }
        //public virtual string DimensionVal { get; set; }
        public virtual CostCenterModel CostCenterMod { get; set; } // Model for the cost center
        public virtual DimensionModel DimensionMod { get; set; } // Model for the dimension
        public virtual string DocumentName { get; set; }
        public virtual string UsageCode { get; set; }

    }

    // Model representing a document voucher sub-entry to be sent
    public class DocumentVoucherSubSendModel
    {
        public int? DocumentVoucherSubID { get; set; } // Unique identifier for the sub-entry
        public int? DocumentVoucherID { get; set; }
        public decimal? Amount { get; set; }
        public int? DocumentID { get; set; }
        public string Reference { get; set; }

    }
}