using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    public class MileageShortageReceiptModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<MileageShortageReceiptModel>(Json);
        }
        public MileageShortageReceiptModel()
        {
            NatureOfTransaction = "Mileage Shortage";
        }
        public int? MileageShortageReceiptID { get; set; }
        [Required]
        public string ReceiptMode { get; set; }
        public string Reference { get; set; }
        public string TripNo { get; set; }
        public long? TripID { get; set; }
        public int? AssetTransferID { get; set; }
        public int? RequestApprovalHistoryID { get; set; }
        public decimal? RunningKM { get; set; }
        public decimal? ConsumedFuel { get; set; }
        public int? DriverID { get; set; }
        public decimal? AllottedMileage { get; set; }
        public decimal? AllottedDistance { get; set; }
        public decimal? FuelPrice { get; set; }
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public override decimal? TotalAmount { get; set; } = 0;
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
    }
}