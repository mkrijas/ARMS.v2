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
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public DateTime? TripDate { get; set; }
        public decimal? Mileage { get; set; }
        public int? RunKM { get; set; }
        public decimal? Fuel { get; set; }                
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class TripInfoModel
    {
        public long? TripID { get; set; }
        public string TripNumber { get; set; }
        public string Truck { get; set; }
        public string Driver { get; set; }
        public string Mileage { get; set; }
        public int? RunKM { get; set; }
        public decimal? Fuel { get; set; }
        public string Gcs { get; set; }
    }
    public class TripFuelModel
    {
        public TripFuelModel()
        {
            UserInfo = new();
        }
        public int? TripFuelID { get; set; }
        [Required]
        public DateTime? EntryDate { get; set; }
        [Required]
        public long? TripID { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public int? FuelItemID { get; set; }
        [Required]
        public decimal? RatePerLitre { get; set; }
        [Required]
        public decimal? TotalAmount { get; set; }
        [Required]
        public decimal? Quantity { get; set; }
        public bool IsPurchase { get; set; } = false;
        public int? PurchaseID { get; set; }
        public int? InvTranID { get; set; }
        public  TaxPurchaseModel PurchaseEntry { get; set; }
        public virtual InventoryBaseModel IssueEntry { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public PartyModel PartyBranch { get; set; } = new();
        public int? Dimension { get; set; }
        public int? Costcenter { get; set; }
        public string UsageID { get; set; }
        public string InvoiceNo { get; set; }
        // tax Purcahse model

    }
}
