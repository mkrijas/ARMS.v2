using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IOpInventoryReleaseService
    {
        OpInventoryReleaseModel Update(OpInventoryReleaseModel model);  //Edit
        OpInventoryReleaseModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<OpInventoryReleaseModel> Select();
        IEnumerable<OpInventoryReleaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<OpInventoryReleaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        int Approve(int? OpInventoryReleaseID, string UserID, string Remarks);  //Approve
        int Reverse(int? SundryReceiptID, string UserID, string Remarks);  //Reverse
        IEnumerable<OpInventoryReleaseSubModel> GetSub(int? ID);
    }
}