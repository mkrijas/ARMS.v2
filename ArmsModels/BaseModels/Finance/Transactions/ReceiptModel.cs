
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
        public PartyModel PartyInfo { get; set; }
        public int? PartyCoa { get; set; }
        public string ReceiptType { get; set; } // Advance/Deposit/Settlement        
        public string ReceiptMode { get; set; } // Cash/Bank
        public string ReceiptTool { get; set; } // (Cheque/NEFT)
        public string ArdCode { get; set; }
        public int? ReceiptCoa { get; set; }//ac cash ac
        public string Reference { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool IsRealized { get; set; }
        public List<BillsReceiptModel> Bills { get; set; }

    }

    public class BillsReceiptModel
    {
        //outstading bills tick 
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

    public class OutstandingPaymentModel
    {
        public int? OpID { get; set; }
        public string PaymentTransactionType { get; set; }
        public int? PaymentTransactionID { get; set; }
        public PartyModel PartyInfo { get; set; }
        public decimal? InitialAmount { get; set; }
        public decimal? OutstandingAmount { get; set; }
        public string DocNumber { get; set; }
        public virtual string BranchName { get; set; }
        public DateTime? DocDate { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? ReferenceDate { get; set; }
    }
}

