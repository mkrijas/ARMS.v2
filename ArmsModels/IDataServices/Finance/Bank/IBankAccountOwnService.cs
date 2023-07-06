using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IBankAccountOwnService
    {       
        OwnBankModel Update(OwnBankModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<OwnBankModel> Select();
        OwnBankModel SelectByID(int ID);
        OwnBankModel SelectByCode(string BankCode);       
        int? GetBankChargeCoaID(int? BankID);
        int? GetBankAccountCoaID(int? BankID);
        int? GetProcessingFeeCoaID(int? BankID);
    }
}
