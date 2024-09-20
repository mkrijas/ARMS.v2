using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class CreditDebitTransferModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CreditDebitTransferModel>(Json);
        }

        public int? ID { get; set; }
        public virtual string BranchName { get; set; }
        [Required]
        public int? TransactionMode { get; set; } //  -1 - Credit, 1 - Debit
        public virtual string? TransactionModeDisplay
        {
            get
            {
                if (TransactionMode == null)
                    return null;

                return TransactionMode.Value < 0 ? "Credit" : "Debit";
            }
        }
        [Required]
        public string OriginPartyType { get; set; } // Supplier/Customer/Renter/SisterConcern
        [Required]
        //public string OriginSubArdCode { get; set; }
        public SubArdCodeModel OriginSubArdCode { get; set; } = new();
        [Required]
        public PartyModel OriginParty { get; set; } = new();
        [Required]
        public int? OriginCoaID { get; set; }
        [Required]
        public string TargetPartyType { get; set; } // Supplier/Customer/Renter/SisterConcern
        [Required]
        //public string TargetSubArdCode { get; set; }
        public SubArdCodeModel TargetSubArdCode { get; set; } = new();
        [Required]
        public PartyModel TargetParty { get; set; } = new();
        [Required]
        public int? TargetCoaID { get; set; }
    }
}