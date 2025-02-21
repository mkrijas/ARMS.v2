using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Core.BaseModels.Finance;

namespace ArmsModels.BaseModels
{
    // Model representing a debit or credit note transaction
    public class DrCrNoteModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DrCrNoteModel>(Json);
        }
        public int? DrCrNoteID { get; set; } // Unique identifier for the debit/credit note
        [Required]
        public string DrCrType { get; set; }  // Debit Note , Credit Note 
        [Required]
        public bool IsRoundOff { get; set; }  // DrCr Note, RoundOff
        [Required]
        public string BusinessNature { get; set; } = "Select";// "Suppier,Customer,Renter"
        [Required]
        public PartyModel Party { get; set; } // Information about the party associated with the note
        [Required]
        public int? PartyCoaID { get; set; }
        [Required]
        public CancellationReasonCode Reason { get; set; } // Reason for issuing the note
        public string Reference { get; set; }
        public int? OriginalTransactionID { get; set; } // Transaction against which this issuing
        public string OriginalTranDocNumber { get; set; } // auto complete if possible or confirm the number
        public DateTime? OriginalTranDocDate { get; set; }
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new(); // List of particulars associated with the note
        public List<TaxPurchaseItemModel> Items { get; set; } = new(); // List of items associated with the note
    }

    // Model representing bill information
    public class BillInfoModel
    {
        public int? PID { get; set; } // Unique identifier for the bill
        public string BillType { get; set; }
        public int? BillID { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new();
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
    }

}
