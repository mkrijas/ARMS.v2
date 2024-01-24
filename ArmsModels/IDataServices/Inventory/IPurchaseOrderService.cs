using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IPurchaseOrderService
    {
        PurchaseOrderModel Update(PurchaseOrderModel model);  //Edit
        PurchaseOrderModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<PurchaseOrderModel> SelectPending(int BranchID);
        IEnumerable<PurchaseOrderModel> PendingForGrn(int BranchID);
        IEnumerable<PurchaseOrderModel> SelectByStore(int StoreID);
        int Approve(int POID, string UserID, string Remarks);  //Approve
        int Reverse(int POID, string UserID);  //Reverse
        int CancelOrder(int POID, string UserID, string Remarks);  //Cancel
        IEnumerable<InventoryItemEntryModel> GetItemEntries(int POID);
        IEnumerable<InventoryItemEntryModel> GetItemEntriesPO(int POID);
    }
}