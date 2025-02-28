using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InventoryItemReOrderLevelService : IInventoryItemReOrderLevelService
    {
        IDbService Iservice;
        public InventoryItemReOrderLevelService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update the reorder level for inventory items
        public InventoryItemReOrderLevelModel Update(InventoryItemReOrderLevelModel model)
        {
            List<InventoryItemReOrderLevelTableItemsModel> ItemsListFormated = new();
            foreach (var item in model.ReOrderLevelList)
            {
                ItemsListFormated.Add(new()
                {
                    ID = item.ID,
                    StoreID = item.Store.StoreID,
                    InventoryItemID = item.InventoryItem.InventoryItemID,
                    MinQty = item.MinQty,
                    ReOrderLevel = item.ReOrderLevel

                });
            }
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               
               new SqlParameter("@Items", ItemsListFormated.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ReOrderLevel.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select a reorder level by its ID
        public InventoryItemReOrderLevelModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryItemReOrderLevelModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ReOrderLevel.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select reorder levels by inventory item ID and branch ID
        public IEnumerable<InventoryItemReOrderLevelModel> SelectByItem(int? InventoryItemID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByItem"),
               new SqlParameter("@ItemID",InventoryItemID),
               new SqlParameter("@BranchID",BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ReOrderLevel.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to map data record to InventoryItemReOrderLevelModel
        private InventoryItemReOrderLevelModel GetModel(IDataRecord dr)
        {
            return new InventoryItemReOrderLevelModel
            {
                ID = dr.GetInt32("ID"),
                Store = new StoreModel()
                {
                    StoreID = dr.GetInt32("StoreID"),
                    StoreName = dr.GetString("StoreName"),
                },
                InventoryItem = new InventoryItemModel()
                {
                    InventoryItemID = dr.GetInt32("ItemID"),
                    InventoryItemCode = dr.GetString("InventoryItemCode"),
                },
                MinQty = dr.GetDecimal("MinQty"),
                ReOrderLevel = dr.GetDecimal("ReOrderLevel"),
                InhandQty = dr.GetDecimal("InhandQty"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
    }
}
