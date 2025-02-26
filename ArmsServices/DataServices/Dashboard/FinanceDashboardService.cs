using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;

namespace ArmsServices.DataServices
{
    // Service class for managing financial dashboard data
    public class FinanceDashboardService : IFinanceDashboardService
    {
        IDbService Iservice;
        public FinanceDashboardService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Retrieves the account balance for a specific COA ID, branch ID, and date
        public AccountBalanceModel GetAccountBalance(int? CoaID, int? BranchID, DateTime? Date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CoaID", CoaID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Date",Date)
            };
            AccountBalanceModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.AccountBalance.Select]", parameters))
            {
                model = GetAccountBalance(dr);
            }
            return model;
        }

        // Helper method to map data from the database to an AccountBalanceModel
        private AccountBalanceModel GetAccountBalance(IDataRecord dr)
        {
            return new AccountBalanceModel
            {
                CoaID = dr.GetInt32("CoaID"),
                Date = dr.GetDateTime("Date"),
                TotalAmount = dr.GetDecimal("Result")
            };
        }

        // Retrieves a list of payable due balances for a specific branch
        public IEnumerable<DueBalanceModel> GetPayableDueBalance(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "PayableDues"),
               new SqlParameter("@BranchID", BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DueBalance.Select]", parameters))
            {
                yield return new DueBalanceModel()
                {
                    MID = dr.GetInt32("MID"),
                    BranchID =  dr.GetInt32("BranchID"),
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                    Amount = dr.GetDecimal("Amount"),
                    InvoiceNo = dr.GetString("InvoiceNo"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    DueDays = dr.GetInt32("DueDays")
                };
            }
        }

        // Retrieves a list of receivable due balances for a specific branch
        public IEnumerable<DueBalanceModel> GetReceivableDueBalance(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ReceivableDues"),
               new SqlParameter("@BranchID", BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DueBalance.Select]", parameters))
            {
                yield return new DueBalanceModel()
                {
                    MID = dr.GetInt32("MID"),
                    BranchID = dr.GetInt32("BranchID"),
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                    Amount = dr.GetDecimal("Amount"),
                    InvoiceNo = dr.GetString("ReferenceDocNo"),
                    InvoiceDate = dr.GetDateTime("ReferenceDocDate"),
                    DueDays = dr.GetInt32("DueDays")
                };
            }
        }

        // Retrieves a list of bank account balances for a specific branch
        public IEnumerable<BankAccountBalanceModel> GetBankAccountBalance(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Bank.AccountBalance.Select]", parameters))
            {
                yield return new BankAccountBalanceModel()
                {
                    AccountNumber = dr.GetString("AccountNumber"),
                    BankTitle = dr.GetString("BankTitle"),
                    Amount = dr.GetDecimal("Amount")
                };
            }
        }

        // Retrieves a list of cash account balances for a specific branch
        public IEnumerable<CashAccountBalanceModel> GetCashAccountBalance(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Cash.AccountBalance.Select]", parameters))
            {
                yield return new CashAccountBalanceModel()
                {
                    Title = dr.GetString("Title"),
                    Amount = dr.GetDecimal("Amount")
                };
            }
        }

        // Retrieves a list of expenses for a specific branch
        public List<DashboardModel> SelectExpenses(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID)
            };
            return GetChartData(parameters);
        }

        // Helper method to retrieve chart data based on provided parameters
        private List<DashboardModel> GetChartData(List<SqlParameter> parameters)
        {
            List<DashboardModel> list = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Expenses.Select]", parameters))
            {
                DashboardModel model = GetDashboardModel(dr);
                list.Add(model);
            }
            return list;
        }

        // Helper method to map data from the database to a DashboardModel
        private DashboardModel GetDashboardModel(IDataRecord dr)
        {
            return new DashboardModel
            {
                Label = dr.GetString("DataLabel"),
                Data = dr.GetInt32("TotalData"),
                DateList = dr?.GetDateTime("DateList"),
                Total = dr.GetDecimal("Total"),
            };
        }
    }
}