using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public class InventoryItemLockService : IInventoryItemLockService
    {
        IDbService Iservice;

        public InventoryItemLockService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a GST item by ID
        public int Delete(int ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LockID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Item.Lock.Delete]", parameters);
        }

        public IEnumerable<InventoryItemLockModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Lock.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InventoryItemLockModel SelectByItem(int ItemID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByItem"),
               new SqlParameter("@ItemID", ItemID)
            };
            InventoryItemLockModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Lock.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public InventoryItemLockModel SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LockID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryItemLockModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Lock.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update an existing GstItemModel record
        public InventoryItemLockModel Update(InventoryItemLockModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LockID", model.LockID),
               new SqlParameter("@ItemID", model.ItemID),
               new SqlParameter("@LockDays", model.LockDays),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Lock.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Private method to convert an IDataRecord to a GstItemModel
        private InventoryItemLockModel GetModel(IDataRecord dr)
        {
            return new InventoryItemLockModel
            {
                LockID = dr.GetInt32("LockID"),
                ItemID = dr.GetInt32("ItemID"),
                ItemCode = dr.GetString("ItemCode"),
                ItemDescription = dr.GetString("ItemDescription"),
                ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                LockDays = dr.GetInt32("LockDays"),
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