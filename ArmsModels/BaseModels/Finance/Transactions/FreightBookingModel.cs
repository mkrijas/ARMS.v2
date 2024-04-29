using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class ConsolidatedDraftBillModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ConsolidatedDraftBillModel>(Json);
        }
        public int? DraftBillID { get; set; }
        [Required]
        public DateTime? DocFromDate { get; set; }
        [Required]
        public DateTime? DocToDate { get; set; }
        public PartyModel Party { get; set; } = new();     
        [Required]
        public OrderModel Order { get; set; }
        [Required]
        public TariffTypeModel TariffType { get; set; }
        [ValidateComplexType]
        public List<GcTariffModel> BookedGCs { get; set; }
    }

    public class ProformaInvoiceModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ProformaInvoiceModel>(Json);
        }
        public int? ProformaInvoiceID { get; set; }
        public int? DraftBillID { get; set; }
        public int? OrderID { get; set; }
        public decimal? FreightAmount { get; set; }
        public PartyModel Party { get; set; }
        public int? PartyCoa { get; set; }
        public TariffTypeModel TariffType { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public string Reference { get; set; }
        public GstModel Gst { get; set; } = new();
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public List<GcTariffModel> BookedGCs { get; set; }
    }

    public class BillingModel : ProformaInvoiceModel
    {
        public int? BillingID { get; set; }
    }

    public class GcTariffModel
    {
        [Required]
        public long? GcTariffID { get; set; }
        public long? GcID { get; set; }
        public int? TariffID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public decimal? Deduction { get; set; } = 0;
        public int? ConsolidatedDraftBillID { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual string BillNumber { get; set; }
        public virtual string PassNumber { get; set; }
        public virtual string ConsigneeName { get; set; }
        public virtual decimal? BillQuantity { get; set; }
        public virtual string GcNumber { get; set; }
        public virtual long? TripNumber { get; set; }
        public virtual string TariffTypeName { get; set; }
        public virtual bool IsChecked { get; set; }
    }

    public class RefModel
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceRefNumber { get; set; }
    }
}