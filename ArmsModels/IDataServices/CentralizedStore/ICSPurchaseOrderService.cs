using System.Collections.Generic;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ICSPurchaseOrderService
    {
        CSPurchaseOrderModel Update(CSPurchaseOrderModel model);  //Edit
        CSPurchaseOrderModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<CSPurchaseOrderModel> SelectPending(int BranchID);
        IEnumerable<CSPurchaseOrderModel> PendingForGrn(int BranchID);
        int Approve(int POID, string UserID, string Remarks);  //Approve
        int Reverse(int POID, string UserID);  //Reverse
        int CancelOrder(int POID, string UserID, string Remarks);  //Cancel
        IEnumerable<CSItemEntryModel> GetItemEntries(int POID);
        IEnumerable<CSItemEntryModel> GetItemEntriesPO(int POID);
    }
}