using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class AccountBalanceModel
    {
        public int? CoaID { get; set; }
        public DateTime? Date { get; set; } = DateTime.Today;
        public decimal? TotalAmount { get; set; }
    }

    public class DueBalanceModel
    {
        public int? MID { get; set; }
        public int? BranchID { get; set;}
        public int? PartyID { get; set; }
        public string TradeName { get; set; }
        public decimal? Amount { get; set;}
        public decimal? TotalAmount { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? DueDays { get; set; } // Days
    }

    public class BankAccountBalanceModel
    {
        public string AccountNumber { get; set; }
        public string BankTitle { get; set; }
        public decimal? Amount { get; set; }
    }

    public class CashAccountBalanceModel
    {
        public string Title { get; set; }
        public decimal? Amount { get; set; }
    }
}