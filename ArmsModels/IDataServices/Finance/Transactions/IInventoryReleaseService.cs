using ArmsModels.BaseModels.Finance.Transactions;
using System.Collections.Generic;
using System;
using ArmsModels.BaseModels;
using System.Data.SqlClient;
using System.Data;

namespace ArmsServices.DataServices.Finance.Transactions
{
    public interface IInventoryReleaseService
    {
        InventoryReleaseModel Update(InventoryReleaseModel model);  //Edit
        InventoryReleaseModel SelectByID(int? ID);
        int Delete(int? ID, bool IsUsedItem, string UserID);  //Delete
        IEnumerable<InventoryReleaseModel> Select();
        public IEnumerable<InventoryReleaseModel> SelectByStoreID(int? StoreID);
        public IEnumerable<InventoryReleaseModel> SelectByStoreAndBranchID(int? StoreID,int? BranchID);
        IEnumerable<InventoryReleaseModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<InventoryReleaseModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<InventoryReleaseSubViewModel> GetRequstSub(int? ID, int? StoreID);
        IEnumerable<InventoryReleaseSubViewModel> GetRequstSubUsed(int? ID, int? StoreID);
        IEnumerable<InventoryReleaseSubViewModel> GetRequstSubReadOnly(int? ID, int? StoreID);
        public int Approve(int? RID, string UserID, string Remarks);
        
    }
}