using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    // Model representing a shortage or damage compensation transaction
    public class ShortageDamageCompensationModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShortageDamageCompensationModel>(Json);
        }
        public ShortageDamageCompensationModel()
        {
            NatureOfTransaction = "Damage Compensation";
        }
        public int? DamageID { get; set; } // Unique identifier for the damage record
        [Required]
        public long? GcSetID { get; set; }
        [Required]
        public int? DriverID { get; set; }
        public string DriverName { get; set; }
        [Required]
        public override decimal? TotalAmount { get; set; }
        [Required]
        public decimal? DamagedQty { get; set; }
        public string Reference { get; set; }
        public virtual long? TripID { get; set; }
        public virtual string SetGcNumber { get; set; }

    }
}