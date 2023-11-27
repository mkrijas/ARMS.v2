using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IBankAccountService
    {       
        BankAccountModel Update(BankAccountModel model);  //Edit
        int Delete(int? BankAccountID, string UserID);  //Delete
        IEnumerable<BankAccountModel> Select();
        BankAccountModel SelectByID(int ID);
        BankAccountModel SelectByPartyBranch(int PartyBranchID);
    }
}