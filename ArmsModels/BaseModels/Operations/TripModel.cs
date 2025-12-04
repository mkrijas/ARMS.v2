using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents a trip in the transportation system
    public class TripModel
    {
        public TripModel()
        {
            UserInfo = new();
        }

        public long? TripID { get; set; } // Unique identifier for the trip (nullable)
        [Required]
        public int? TruckID { get; set; }
        public int? DriverID { get; set; }
        public string TripPrefix { get; set; }
        public long? TripNumber { get; set; }
        public string TripNumberDisplay { get { return TripPrefix + TripNumber.ToString().PadLeft(4, '0'); } }
        public bool? StartWithLoading { get; set; } = false;
        [Required]
        public int? BranchID { get; set; }
        [Required]
        [NotFutureDateTime(ErrorMessage = "Trip date and time cannot be in the future.")]
        public DateTime? TripDate { get; set; }
        //[ExpressiveAnnotations.Attributes.RequiredIf("StartWithLoading == false", ErrorMessage ="Please select destination.")]
        //public int? DestinationID { get; set; }
        [Required]
        public DateTime? EventTime { get; set; } = DateTime.Now;
        [Required]
        [Notless("TruckID", "EventTime")]
        public long? EventReading { get; set; }
        [Required]
        public int? OriginID { get; set; }
        [Required]
        public int? DestinationID { get; set; }
        public decimal? Mileage { get; set; }
        public int? RunKM { get; set; }
        public decimal? Fuel { get; set; }
        public bool Closed { get; set; }
        public bool IsLocked { get; set; }
        public bool IsMileageOverride { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        [ValidateComplexType]
        public EventModel StartEvent { get; set; }
    }

    // Represents summarized information about a trip
    public class TripInfoModel
    {
        public long? TripID { get; set; }
        public string TripNumber { get; set; }
        public string Truck { get; set; }
        public string Driver { get; set; }
        public decimal? Mileage { get; set; }
        public int? RunKM { get; set; }
        public string RunDuration { get; set; }
        public decimal? Fuel { get; set; }
        public decimal? Freight { get; set; }
        public decimal? Expenses { get; set; }
        public string Gcs { get; set; }
        public bool IsMileageOverride { get; set; }
        public decimal? SettledKm { get; set; }

    }

    // Represents fuel-related information for a trip
    public class TripFuelModel
    {
        DateTime? docdate = DateTime.Today;
        decimal? amount;
        public long? TripFuelID { get; set; }
        public string ItemType { get; set; }
        [Required]
        public DateTime? EntryDate
        {
            get { return docdate; }
            set { docdate = value; this.PurchaseModel.DocumentDate = value; }
        }
        public long? TripID { get; set; }
        public int? AssetTransferID { get; set; }
        public int? RequestApprovalHistoryID { get; set; }
        public int? TruckID { get; set; }
        [Required]
        public int? FuelItemID { get; set; }
        public string FuelItemDescription { get; set; }
        public decimal? RatePerLitre { get; set; }
        public decimal? Amount
        {
            get { return amount; }
            set { amount = value; this.PurchaseModel.TotalAmount = value; }
        }
        [Required]
        public decimal? Quantity { get; set; }
        [Required]
        public bool IsPurchase { get; set; } = false;
        public TaxPurchaseModel PurchaseModel { get; set; } = new(); // Purchase model associated with the fuel record
        public string UsageID { get; set; }
        public int? MID { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public string Narration { get; set; }
        public string RefInvoiceNo { get; set; }
        public bool IsUsedItem { get; set; }
        public string DocNumber { get; set; }
    }

    // Represents an advance payment associated with a trip
    public class TripAdvanceModel
    {
        public TripAdvanceModel()
        {
            UserInfo = new();
        }
        public long? TripAdvanceID { get; set; } // Unique identifier for the trip advance record (nullable)
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public long? TripID { get; set; }
        public long? GcSetID { get; set; }
        [Required]
        public int? DriverID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public int? CoaID { get; set; }
        public string UsageCode { get; set; }
        public string UsageDescription { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
        public string DocType { get; set; }
        public virtual bool IsChecked { get; set; }
        public string TripNo { get; set; }
        public string GcNo { get; set; }
        public string DriverName { get; set; }
        public string DriverMobile { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int RecordStatus { get; set; }

        public DateTime? Docdate = DateTime.Today;
        public string PaymentMode { get; set; }
        public string PaymentArdCode { get; set; }
        public int? PaymentCoaID { get; set; }
        public string PaymentTool { get; set; }
        public decimal? BankCharges { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
        public string Reference { get; set; }
        public string Narration { get; set; }
        public List<SundryReceiptEntryModel> Entries { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Represents an event card associated with a trip
    public class EventCardModel
    {
        public long? EventID { get; set; } // Unique identifier for the event (nullable)
        public string EventName { get; set; }
        public string NextEventName { get; set; }
        public string PlaceName { get; set; }
        public DateTime? EventDateTime { get; set; }
        //public decimal? EventDateTimeDiff { get; set; }
        public string EventDateTimeDiff { get; set; }
        public long? KMReading { get; set; }
        public long? KMReadingDiff { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}