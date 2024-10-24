using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IInventoryGrnService
    {
        InventoryGrnModel Update(InventoryGrnModel model);  //Edit
        InventoryGrnModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        int ToInvoiceCancel(int? ID);  //Delete
        IEnumerable<InventoryGrnModel> SelectPending(int BranchID);
        IEnumerable<InventoryGrnModel> PendingToInvoice(int BranchID);
        IEnumerable<InventoryGrnModel> SelectByStore(int StoreID);
        int Approve(int GrnID, string UserID, string Remarks);  //Approve
        int Reverse(int GrnID, string UserID);  //Reverse
        IEnumerable<InventoryItemEntryModel> GetItemEntries(int GrnID);
    }
}