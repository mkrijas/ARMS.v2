using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Core.BaseModels.Finance;

namespace ArmsModels.BaseModels
{
    public class DrCrNoteModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DrCrNoteModel>(Json);
        }
        public int? DrCrNoteID { get; set; }
        [Required]
        public string DrCrType { get; set; }  // Debit Note , Credit Note 
        [Required]
        public string BusinessNature { get; set; } = "Select";// "Suppier,Customer,Renter"
        [Required]
        public PartyModel Party { get; set; }
        public int? PartyCoaID { get; set; }
        [Required]
        public CancellationReasonCode Reason { get; set; }
        public string Reference { get; set; }
        public int? OriginalTransactionID { get; set; } // Transaction against which this issuing
        public string OriginalTranDocNumber { get; set; } // auto complete if possible or confirm the number
        public DateTime? OriginalTranDocDate { get; set; }
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new();
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
    }

    public class BillInfoModel
    {
        public int? PID { get; set; }
        public string BillType { get; set; }
        public int? BillID { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new();
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
    }

}
