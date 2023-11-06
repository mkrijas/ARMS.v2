using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IOpInventoryReleaseService
    {
        OpInventoryReleaseModel Update(OpInventoryReleaseModel model);
        OpInventoryReleaseModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<OpInventoryReleaseModel> Select();
        IEnumerable<OpInventoryReleaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<OpInventoryReleaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        int Approve(int? OpInventoryReleaseID, string UserID, string Remarks);
        int Reverse(int? SundryReceiptID, string UserID, string Remarks);
        IEnumerable<OpInventoryReleaseSubModel> GetSub(int? ID);
    }
}
