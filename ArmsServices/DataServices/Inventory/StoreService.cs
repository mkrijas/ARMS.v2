using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class StoreService : IStoreService
    {
        IDbService Iservice;
        public StoreService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a store by its ID
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Store.Delete]", parameters);
        }

        // Method to get batch details for a specific batch ID
        public LinkableBatchModel GetBatchDetails(long? BatchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BatchID", BatchID),
               new SqlParameter("@Operation", "GetBatchDetail"),
            };
            foreach (var dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", parameters))
            {
                return new LinkableBatchModel()
                {
                    BatchID = dr.GetInt64("BatchID"),
                    StoreID = dr.GetInt32("StoreID"),
                    LinkableQty = dr.GetDecimal("LinkableQty"),
                    GrnNo = dr.GetString("GrnNo"),
                    PartyID = dr.GetInt32("PartyID")
                };
            }
            return null;
        }

        // Method to get item availability in a specific store
        public InventoryItemModel GetItemAvailability(int StoreID, int ItemID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", StoreID),
               new SqlParameter("@ItemID", ItemID),
               new SqlParameter("@Operation", "GetItemAvailability"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", parameters))
            {
                return new InventoryItemModel()
                {
                    InventoryItemID = ItemID,
                    QtyAvailable = dr.GetDecimal("QtyAvailable"),
                };
            }
            return null;
        }

        // Method to get used item availability in a specific store
        public InventoryItemModel GetUsedItemAvailability(int StoreID, int ItemID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", StoreID),
               new SqlParameter("@ItemID", ItemID),
               new SqlParameter("@Operation", "GetUsedItemAvailability"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", parameters))
            {
                return new InventoryItemModel()
                {
                    InventoryItemID = ItemID,
                    QtyAvailable = dr.GetDecimal("QtyAvailable"),
                };
            }
            return null;
        }

        // Method to process outflow of items from a store
        public int OutFlow(int? StoreID, List<InventoryItemEntryModel> Items, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", StoreID),
               new SqlParameter("@Items", Items.ToDataTable()),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Store.Outflow]", parameters);
        }

        // Method to select all stores
        public IEnumerable<StoreModel> Select()
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select stores by branch ID
        public IEnumerable<StoreModel> SelectByBranch(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ByBranch"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a store by its ID
        public StoreModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", ID)
            };
            StoreModel model = new("branch");
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update a store's detailsc
        public StoreModel Update(StoreModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", model.StoreID),
               new SqlParameter("@BranchID",model.BranchID),
               new SqlParameter("@StoreName",model.StoreName),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to StoreModel
        private StoreModel GetModel(IDataRecord dr)
        {
            return new StoreModel(dr.GetString("BranchName"))
            {
                StoreName = dr.GetString("StoreName"),
                StoreID = dr.GetInt32("StoreID"),
                BranchID = dr.GetInt32("BranchID"),
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