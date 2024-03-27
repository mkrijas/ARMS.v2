using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
    }
}
