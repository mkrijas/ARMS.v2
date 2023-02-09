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
        StoreModel Update(StoreModel model);
        StoreModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<StoreModel> Select();
        IEnumerable<StoreModel> SelectByBranch(int BranchID);
        int OutFlow(int? StoreID, List<InventoryItemEntryModel> Items,string UserID);
        InventoryItemModel GetItemAvailability(int StoreID,int ItemID);
    }
    public class StoreService : IStoreService
    {
        IDbService Iservice;
        public StoreService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Store.Delete]", parameters);
        }

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

        public int OutFlow(int? StoreID, List<InventoryItemEntryModel> Items,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", StoreID),
               new SqlParameter("@Items", Items.ToDataTable()),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Store.Outflow]", parameters);
        }

        public IEnumerable<StoreModel> Select()
        { 
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Select]", null))
            {
                yield return GetModel(dr);
            }
        }

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
