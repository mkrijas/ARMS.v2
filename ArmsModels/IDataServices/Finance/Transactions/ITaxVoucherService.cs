using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices.Finance.Transactions
{
    public interface ITaxVoucherService
    {
        TaxVoucherModel Update(TaxVoucherModel model);
        TaxVoucherModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<TaxVoucherModel> Select();
        IEnumerable<TaxVoucherModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<TaxVoucherModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxVoucherSubModel> GetSub(int? ID);
        int Approve(int? TaxVoucherID, string UserID, string Remarks);
        int Reverse(int? PID, string UserID, string Remarks);
    }
}
