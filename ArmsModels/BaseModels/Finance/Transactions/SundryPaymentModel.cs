using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.Finance.Transactions
{
    public class SundryPaymentModel:TransactionBaseModel
    {
        public int? SundryPaymentID { get; set; }
        [Required]
        public string PaymentMode { get; set; }
        [Required]
        public int? CoaID { get; set; }
        public virtual string AccountName { get; set; }
        public string Reference { get; set; }
        public List<SundryPaymentEntryModel> Entries { get; set; }

    }

    public class SundryPaymentEntryModel
    {
        public long? ID { get; set; }
        public int? ParentID { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public int? CoaID { get;}
        public virtual string AccountName { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string Rederence { get; set; }
    }
}
