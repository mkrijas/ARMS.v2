using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing the account balance
    public class AccountBalanceModel
    {
        public int? CoaID { get; set; } // Identifier for the Chart of Accounts (CoA)
        public DateTime? Date { get; set; } = DateTime.Today;
        public decimal? TotalAmount { get; set; }
    }

    // Model representing the due balance for a party
    public class DueBalanceModel
    {
        public int? MID { get; set; } // Unique identifier for the due balance record
        public int? BranchID { get; set;}
        public int? PartyID { get; set; }
        public string TradeName { get; set; }
        public decimal? Amount { get; set;}
        public decimal? TotalAmount { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? DueDays { get; set; } // Days
    }

    // Model representing the balance of a bank account
    public class BankAccountBalanceModel
    {
        public string AccountNumber { get; set; }
        public string BankTitle { get; set; }
        public decimal? Amount { get; set; }
    }

    // Model representing the balance of a cash account
    public class CashAccountBalanceModel
    {
        public string Title { get; set; }
        public decimal? Amount { get; set; }
    }
}