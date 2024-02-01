using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices.Finance.Transactions
{
    public interface IDocVoucherService
    {
        DocumentVoucherModel Update(DocumentVoucherModel model);  //Edit
        DocumentVoucherModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<DocumentVoucherModel> Select(int? BranchID);
        IEnumerable<DocumentVoucherModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<DocumentVoucherModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<DocumentVoucherSubModel> GetSub(int? ID);
        IEnumerable<DocumentVoucherSubModel> GetNotPostedSubDocuments(int? DocumentTypeID);
        int Approve(int? TaxVoucherID, string UserID, string Remarks);  //Approve
        int Reverse(int? PID, string UserID, string Remarks);  //Reverse
    }
}