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
        TaxVoucherModel Update(TaxVoucherModel model);  //Edit
        TaxVoucherModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<TaxVoucherModel> Select(int? BranchID);
        IEnumerable<TaxVoucherModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<TaxVoucherModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxVoucherSubModel> GetSub(int? ID);
        IEnumerable<TaxVoucherSubModel> GetNotPostedSubDocuments(int? DocumentTypeID);
        int Approve(int? TaxVoucherID, string UserID, string Remarks);  //Approve
        int Reverse(int? PID, string UserID, string Remarks);  //Reverse
    }
}