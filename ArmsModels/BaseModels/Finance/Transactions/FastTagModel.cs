using Microsoft.VisualBasic;
using System;
using System.Transactions;

namespace Core.BaseModels.Finance.Transactions
{
    public class FastTagModel
    {
        public DateTime TransactionDateTime { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public string NumberPlate { get; set; }
        public int TagAccountNumber { get; set; }
        public string Group {  get; set; }
        public string Activity { get; set; }
        public string PlazaCode { get; set; }
        public string Description {  get; set; }
        public string TransactionID { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount {  get; set; }
    }
}