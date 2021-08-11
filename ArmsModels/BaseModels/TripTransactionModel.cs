using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TripTransactionModel
    {
        public TripTransactionModel()
        {
            UserInfo = new();
        }
        public long TransactionID { get; set; }
        public int TransactionTypeID { get; set; }
        public long TripID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? BillDate { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public long FinanceTranID { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TripTransactionTypeModel
    {
        public TripTransactionTypeModel()
        {
            UserInfo = new();
        }
        public int TransactionTypeID { get; set; }
        public string TTName { get; set; }
        public string Unit { get; set; }
        public string Nature { get; set; }
        public bool AllowMultiple { get; set; } = true;
        public string CrDr { get; set; }
        public int FinancialAccountID { get; set; }
        public UserInfoModel UserInfo { get; set; }     
    }
}
