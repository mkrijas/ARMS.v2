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
        CashAccountModel Update(CashAccountModel model);
        int Delete(int? CashAccountID, string UserID);
        int DisableAccount(bool? IsDisable, int? CashAccountID, string UserID);
        IEnumerable<CashAccountModel> Select();
        CashAccountModel SelectByID(int ID);
        IEnumerable<CashAccountModel> SelectByBranch(int BranchID);
        IEnumerable<CashAccountModel> SelectByBranchALL(int BranchID);
    }
}
