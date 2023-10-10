using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.Finance.Transactions
{
    public class TaxVoucherModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TaxVoucherModel>(Json);
        }
        public int? TaxVoucherID { get; set; }
        [Required]
        public AssetDocumentTypeModel DocumentType { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        public bool? IsAgent { get; set; } = false;
        [RequiredIfTrue("IsAgent")]
        public PartyModel Agent { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsAgent == false")]
        public string PaymentMode { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsAgent == false")]
        public string PaymentArdCode { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsAgent == false")]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public virtual decimal? GstRate { get; set; }
        public virtual string AccountName { get; set; }
        public List<TaxVoucherSubModel> TaxVoucherSubList { get; set; } = new();
    }

    public class TaxVoucherSubModel
    {
        public int? TaxVoucherSubID { get; set; }
        public int? TaxVoucherID { get; set; }
        public decimal? Amount { get; set; }
        public int? AssetID { get; set; }
        public string Reference { get; set; }
        public AssetModel Asset { get; set; }

    }

    public class TaxVoucherSubSendModel
    {
        public int? TaxVoucherSubID { get; set; }
        public int? TaxVoucherID { get; set; }
        public decimal? Amount { get; set; }
        public int? AssetID { get; set; }
        public string Reference { get; set; }

    }
}