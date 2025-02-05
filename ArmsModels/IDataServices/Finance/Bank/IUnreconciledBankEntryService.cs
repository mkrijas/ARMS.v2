using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System;

namespace ArmsServices.DataServices
{
    public interface IUnreconciledBankEntryService
    {
        UnReconciledBankEntryModel Update(UnReconciledBankEntryModel model);  //Edit
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<UnReconciledBankEntryModel> Select(int? BankID, bool ShowOnlyUnreconciled);
        IEnumerable<UnReconciledBankEntryModel> SelectByBranch(int? BranchID);        
        ReconciledBankEntryModel GetRecociledInfo(int? UnreconciledEntryID);
        UnReconciledBankEntryModel SelectByID(int? ID);
        ReconciledBankEntryModel Reconcile(ReconciledBankEntryModel model);
        IEnumerable<ReconciledBankEntryModel> SelectAllUnReconciledBank(int? BranchID,int? BankID);
        IEnumerable<ReconciledBankEntryModel> SelectAllReconciledBank(int? BranchID, int? BankID, DateTime? StartDate, DateTime? EndDate);
        ReconciledBankEntryModel UpdateUnReconciledBankEntry(List<ReconcileUpdateModel> lst,string userID);
        List<ReconciledBankSummaryModel> GetReconcilBankSummary(int? BranchID, string ArdCode, DateTime? StartDate, DateTime? EndDate);
    }
}