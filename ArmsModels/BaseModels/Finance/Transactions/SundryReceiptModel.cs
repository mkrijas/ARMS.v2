using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.Finance.Transactions
{
    public class SundryReceiptModel:TransactionBaseModel
    {
        public int? SundryReceiptID { get; set; }
        [Required]
        public string ReceiptMode { get; set; }
        public string ArdCode { get; set; }
        [Required]
        public int? CoaID { get; set; }        
        public string Reference { get; set; }
        public string PayerName { get; set; }
        public string PayerContactNo { get; set; }        
        public List<SundryReceiptEntryModel> Entries { get; set; }
    }

    public class SundryReceiptEntryModel
    {
        public long? ID { get; set; }
        public int? ParentID { get; set; }
        public int? BranchID { get; set; }
        public string UsageCode { get; set; }
        [Required]
        public int? CoaID { get; set; }
        //public virtual string AccountName { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string Rederence { get; set; }
    }
}
