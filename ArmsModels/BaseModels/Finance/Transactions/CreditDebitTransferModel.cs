using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing a credit or debit transfer transaction
    public class CreditDebitTransferModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CreditDebitTransferModel>(Json);
        }

        public int? ID { get; set; } // Unique identifier for the transaction
        public virtual string BranchName { get; set; }
        [Required]
        public int? TransactionMode { get; set; } //  -1 - Credit, 1 - Debit
        // Property to display the transaction mode as a string
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
        public SubArdCodeModel OriginSubArdCode { get; set; } = new(); // Sub ARD code for the origin party
        [Required]
        public PartyModel OriginParty { get; set; } = new(); // Information about the origin party
        [Required]
        public int? OriginCoaID { get; set; }
        [Required]
        public string TargetPartyType { get; set; } // Supplier/Customer/Renter/SisterConcern
        [Required]
        public SubArdCodeModel TargetSubArdCode { get; set; } = new(); // Sub ARD code for the target party
        [Required]
        public PartyModel TargetParty { get; set; } = new(); // Information about the target party
        [Required]
        public int? TargetCoaID { get; set; }
    }
}