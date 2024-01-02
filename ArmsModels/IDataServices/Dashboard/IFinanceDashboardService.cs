using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IFinanceDashboardService
    {
        AccountBalanceModel GetAccountBalance(int? CoaID, int? BranchID, DateTime? Date);
        IEnumerable<DueBalanceModel> GetPayableDueBalance(int? BranchID);
        IEnumerable<DueBalanceModel> GetReceivableDueBalance(int? BranchID);
        IEnumerable<BankAccountBalanceModel> GetBankAccountBalance(int? BranchID);
        IEnumerable<CashAccountBalanceModel> GetCashAccountBalance(int? BranchID);
        List<DashboardModel> SelectExpenses(int? BranchID);
    }
}