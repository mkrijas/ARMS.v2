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
        public string DrCrType { get; set; } // DebitNote Or CreditNote
        [Required]
        public PartyBranchModel PartyBranch { get; set; }
        public int? PartyBranchCoaID { get; set; }        
        public string Reference { get; set; }
        public string ReasonCode { get; set; } // dropdownlist
        public int? OriginalTransactionID { get; set; } // Transaction against which this issuing
        public string OriginalTranDocNumber { get; set; } // auto complete if possible or confirm the number
        public DateTime? OriginalTranDocDate { get; set; }
        public List<TaxPurchaseExpenseModel> Particulars { get; set; }
        public List<TaxPurchaseItemModel> Items { get; set; }
    }


    public class BillInfoModel
    {
        public int? BillID { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public decimal? TotalAmount { get; set; }
    }

}
