using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IBankPostingGroupService
    {
        BankPostingGroupModel Update(BankPostingGroupModel model);
        BankPostingGroupModel SelectByID(int? ID);
        BankPostingGroupModel SelectByBank(int? BankID);
        int Delete(int? ID, string UserID);        
        IEnumerable<BankPostingGroupModel> Select();        
    }
}