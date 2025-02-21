using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

namespace ArmsModels.BaseModels
{
    // Model representing a consolidated draft bill
    public class ConsolidatedDraftBillModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ConsolidatedDraftBillModel>(Json);
        }
        public int? DraftBillID { get; set; } // Unique identifier for the draft bill
        [Required]
        public DateTime? DocFromDate { get; set; }
        [Required]
        public DateTime? DocToDate { get; set; }
        public TruckModel Truck { get; set; } = new(); // Information about the truck associated with the draft bill
        public PartyModel Party { get; set; } = new(); // Information about the party associated with the draft bill
        [Required]
        public OrderModel Order { get; set; } // Information about the order associated with the draft bill
        [Required]
        public TariffTypeModel TariffType { get; set; } // Information about the tariff type associated with the draft bill
        [ValidateComplexType]
        public List<GcTariffModel> BookedGCs { get; set; } // List of booked GC tariffs associated with the draft bill
    }

    // Model representing a proforma invoice
    public class ProformaInvoiceModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ProformaInvoiceModel>(Json);
        }
        public int? ProformaInvoiceID { get; set; } // Unique identifier for the proforma invoice
        public int? DraftBillID { get; set; }
        public int? OrderID { get; set; }
        public decimal? FreightAmount { get; set; }
        public PartyModel Party { get; set; } // Information about the party associated with the proforma invoice
        public int? PartyCoa { get; set; }
        public TariffTypeModel TariffType { get; set; } // Information about the tariff type associated with the proforma invoice
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public string Reference { get; set; }
        public GstModel Gst { get; set; } = new(); // Information about the GST associated with the proforma invoiced
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public List<GcTariffModel> BookedGCs { get; set; } // List of booked GC tariffs associated with the proforma invoice
    }

    // Model representing a billing transaction
    public class BillingModel : ProformaInvoiceModel
    {
        public int? BillingID { get; set; } // Unique identifier for the billing transaction
    }

    // Model representing a GC tariff
    public class GcTariffModel
    {
        [Required]
        public long? GcTariffID { get; set; } // Unique identifier for the GC tariff
        public long? GcID { get; set; }
        public int? TariffID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public decimal? Deduction { get; set; } = 0;
        public int? ConsolidatedDraftBillID { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual string TruckRegNo { get; set; }
        public virtual string BillNumber { get; set; }
        public virtual string PassNumber { get; set; }
        public virtual string ConsigneeName { get; set; }
        public virtual decimal? BillQuantity { get; set; }
        public virtual string GcNumber { get; set; }
        public virtual long? TripNumber { get; set; }
        public virtual string TariffTypeName { get; set; }
        public virtual bool IsChecked { get; set; }
        public virtual string PaymentMode { get; set; }
        public virtual string DistrictName { get; set; }
        public virtual string StateName { get; set; }
    }

    // Model representing a reference
    public class RefModel
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceRefNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
    }
}