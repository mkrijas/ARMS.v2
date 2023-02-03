using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class DrCrNoteModel : TransactionBaseModel
    {
        public int? DrCrNoteID { get; set; }
        [Required]
        public string DrCrType { get; set; } // Debit Note , Credit NOte 
        [Required]
        public PartyModel Party { get; set; }       
        public int? PartyCoaID { get; set; }        
        public string Reference { get; set; }
        public string ReasonCode { get; set; } // dropdownlist
        public int? OriginalTransactionID { get; set; } // Transaction against which this issuing
        public string OriginalTranDocNumber { get; set; } // auto complete if possible or confirm the number
        public DateTime? OriginalTranDocDate { get; set; }
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new();
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
    }

    public class BillInfoModel
    {
        public int? PID { get; set; }
        public int? BillID { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public decimal? TotalAmount { get; set; }
    }

}
