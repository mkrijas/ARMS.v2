using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ArmsServices.DataServices.Inventory
{
    public interface IInventoryRequestService
    {
        InventoryRequestModel Update(InventoryRequestModel model);  //Edit
        InventoryRequestModel ClosedInventory(InventoryRequestModel model);  //Edit
        InventoryRequestModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<InventoryRequestModel> Select();
        IEnumerable<InventoryRequestModel> SelectByStore(int? StoreID, int? BranchID);
        IEnumerable<InventoryRequestModel> SelectByTruckID(int? TruckID);
        IEnumerable<InventoryRequestModel> SelectRequestReleaseByTruckID(int? TruckID);
        IEnumerable<InventoryRequestModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<InventoryRequestModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<InventoryItemEntryModel> GetSub(int? ID);
        //decimal? GetItemRequestStatus(int? RequestID, int? ItemID);
    }
}