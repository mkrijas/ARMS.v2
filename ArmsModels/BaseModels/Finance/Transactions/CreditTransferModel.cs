using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class CreditTransferModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CreditTransferModel>(Json);
        }

        public int? ID { get; set; }
        [Required]
        public int? TransactionMode { get; set; } //  -1 - Credit, 1 - Debit
        [Required]
        public string OriginPartyType { get; set; } // Supplier/Customer/Renter/SisterConcern
        [Required]
        public string OriginSubArdCode { get; set; }
        [Required]
        public PartyModel OriginParty { get; set; }
        [Required]
        public int? OriginCoaID { get; set; }
        [Required]
        public string TargetPartyType { get; set; } // Supplier/Customer/Renter/SisterConcern
        [Required]
        public string TargetSubArdCode { get; set; }
        [Required]
        public PartyModel TargetParty { get; set; }
        [Required]
        public int? TargetCoaID { get; set; }
        
    }
}
