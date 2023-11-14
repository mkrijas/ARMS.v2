using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ICashAccountService
    {
        CashAccountModel Update(CashAccountModel model);  //Edit
        int Delete(int? CashAccountID, string UserID);  //Delete
        int DisableAccount(bool? IsDisable, int? CashAccountID, string UserID);  // Enable/Disable
        IEnumerable<CashAccountModel> Select();
        CashAccountModel SelectByID(int ID);
        IEnumerable<CashAccountModel> SelectByBranch(int BranchID);
        IEnumerable<CashAccountModel> SelectByBranchALL(int BranchID);
    }
}