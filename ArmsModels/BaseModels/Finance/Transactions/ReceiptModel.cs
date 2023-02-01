
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
        public int? PartyCoaID { get; set; }
        public string ReceiptType { get; set; }  // Advance/Deposit/Settlement        
        public string ReceiptMode { get; set; }  // Cash/Bank
        public string ReceiptTool { get; set; }  // (Cheque/NEFT)
        public string ReceiptArdCode { get; set; }
        public decimal? BankCharges { get; set; }
        public int? ReceiptCoaID { get; set; }  //ac cash ac
        public string Reference { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool IsRealized { get; set; }
        public string remarks { get; set; }
       
        public List<BillsReceiptModel> Bills { get; set; } = new();

    }

    public class BillsReceiptModel
    {
        //outstading bills tick 
        decimal? _ReceiptAmount;
        public int? BrID { get; set; }
        public int? MID { get; set; }
        public bool? IsMemo { get; set; } = false;
        //public decimal? ReceiptAmount { get; set; }
        public decimal? ReceiptAmount
        {
            get { return _ReceiptAmount; }
            set
            {
                // _PayAmount = (Math.Abs(value??0) > Math.Abs(OutstandingAmount ?? 0) || Math.Abs(value + OutstandingAmount??0) > Math.Abs(OutstandingAmount??0) ? -OutstandingAmount : value);
                _ReceiptAmount = value;
            }
        }
        public virtual string BranchName { get; set; }
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

