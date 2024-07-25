using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
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
        public int? DamageID { get; set; }
        public string GcNumber { get; set; }
        public long? GcID { get; set; }
        public int? DriverID { get; set; }
        public string DriverName { get; set; }
        public override decimal? TotalAmount { get; set; } = 0;
        public decimal? DamagedQty { get; set; }
        public string Reference { get; set; }
    }
}