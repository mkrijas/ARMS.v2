using ArmsModels.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    public class MileageShortageReceiptModel : TransactionBaseModel
    {

        public int? MileageShortageReceiptID { get; set; }
        [Required]
        public string ReceiptMode { get; set; }
        [Required]
        public string ArdCode { get; set; }
        public string Reference { get; set; }
        public long? TripID { get; set; }
        public int? DriverID { get; set; }
        public decimal? AllottedMileage { get; set; }
        public decimal? AllottedDistance { get; set; }
        public decimal? FuelPrice { get; set; }

    }
}
