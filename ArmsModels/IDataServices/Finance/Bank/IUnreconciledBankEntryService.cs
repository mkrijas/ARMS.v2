
using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace ArmsServices.DataServices
{

    public interface IUnreconciledBankEntryService
    {
        UnReconciledBankEntryModel Update(UnReconciledBankEntryModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<UnReconciledBankEntryModel> Select(int? BankID, bool ShowOnlyUnreconciled);
        IEnumerable<UnReconciledBankEntryModel> SelectByBranch(int? BranchID);        
        ReconciledBankEntryModel GetRecociledInfo(int? UnreconciledEntryID);
        UnReconciledBankEntryModel SelectByID(int? ID);
        ReconciledBankEntryModel Reconcile(ReconciledBankEntryModel model);
        IEnumerable<ReconciledBankEntryModel> SelectAllUnReconciledBank(int? BranchID,int? BankID);
        ReconciledBankEntryModel UpdateUnReconciledBankEntry(ReconciledBankEntryModel reconciledBankEntry);
    }
}
