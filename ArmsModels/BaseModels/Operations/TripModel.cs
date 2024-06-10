using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TripModel
    {
        public TripModel()
        {
            UserInfo = new();
        }

        public long? TripID { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public int? DriverID { get; set; }
        public string TripPrefix { get; set; }
        public long? TripNumber { get; set; }
        public string TripNumberDisplay { get { return TripPrefix + TripNumber.ToString().PadLeft(4,'0'); } }
        public bool? StartWithLoading { get; set; } = false;
        [Required]
        public int? BranchID { get; set; }
        [Required]
        [NotFutureDateTime(ErrorMessage = "Trip date and time cannot be in the future.")]
        public DateTime? TripDate { get; set; }
        //[ExpressiveAnnotations.Attributes.RequiredIf("StartWithLoading == false", ErrorMessage ="Please select destination.")]
        //public int? DestinationID { get; set; }
        public decimal? Mileage { get; set; }
        public int? RunKM { get; set; }
        public decimal? Fuel { get; set; }
        public bool Closed { get; set; }
        public bool IsLocked { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        [ValidateComplexType]
        public EventModel StartEvent { get; set; }
    }

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
    }

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
        public TaxPurchaseModel PurchaseModel { get; set; } = new();
        public string UsageID { get; set; }
        public int? MID { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public string Narration { get; set; }
        public string RefInvoiceNo { get; set; }
        public bool IsUsedItem { get; set; }
    }

    public class TripAdvanceModel
    {
        public long? TripAdvanceID { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public long? TripID { get; set; }
        [Required]
        public int? DriverID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public int? CoaID { get; set; }
        public string UsageCode { get; set; }
        public string UsageDescription { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
        public virtual bool IsChecked { get; set; }
        public string TripNo { get; set; }
        public string DriverName { get; set; }
        public string DriverMobile { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }

    public class EventCardModel
    {
        public long? EventID { get; set; }
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