using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IStoreService
    {
        StoreModel Update(StoreModel model);  //Edit
        StoreModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<StoreModel> Select();
        IEnumerable<StoreModel> SelectByBranch(int BranchID);
        int OutFlow(int? StoreID, List<InventoryItemEntryModel> Items, string UserID);
        InventoryItemModel GetItemAvailability(int StoreID, int ItemID);
        InventoryItemModel GetUsedItemAvailability(int StoreID, int ItemID);
        LinkableBatchModel GetBatchDetails(long? BatchID);
    }
}