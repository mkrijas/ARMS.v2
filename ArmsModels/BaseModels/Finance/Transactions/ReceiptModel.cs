
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ReceiptModel : TransactionBaseModel
    {
        public int? ReceiptID { get; set; }        
        public PartyBranchModel PartyBranchInfo { get; set; }
        public int? PartyBranchCoa { get; set; }
        public string ReceiptType { get; set; }// Advance/Deposit/Settlement
        public string ReceiiptMode { get; set; }// Cash/Bank (Cheque/NEFT)
        public int? ReceiptCoa { get; set; }
        public string Reference { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool IsRealized { get; set; }

    }


    public class BillsReceiptModel
    {
        public int? BrID { get; set; }
        public int? BoID { get; set; }
        public decimal? ReceiptAmount { get; set; }

        public virtual string DocNumber { get; set; }
        public virtual string BranchName { get; set; }
        public virtual DateTime? DocDate { get; set; }
        public virtual int? BranchID { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
    }
}

