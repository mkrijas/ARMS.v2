
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InventoryItemGroupService : IInventoryItemGroupService
    {
        IDbService Iservice;
        public InventoryItemGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete an inventory item group by its ID
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ItemGroupID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Item.ItemGroup.Delete]", parameters);
        }

        // Method to search for inventory item groups by name
        public IEnumerable<InventoryItemGroupModel> SearchByName(string GroupName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "filter"),
               new SqlParameter("@ItemGroupDescription",GroupName),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ItemGroup.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select an inventory item group by its ID
        public InventoryItemGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ItemGroupID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryItemGroupModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ItemGroup.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select inventory item groups by a specific group ID
        public IEnumerable<InventoryItemGroupModel> SelectByGroup(int? GroupID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByGroup"),
               new SqlParameter("@InventoryGroupID",GroupID),
                };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ItemGroup.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update an inventory item group
        public InventoryItemGroupModel Update(InventoryItemGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ItemGroupID", model.ItemGroupID),
               new SqlParameter("@ItemGroupDescription",model.ItemGroupDescription),
               new SqlParameter("@Group2ID",model.Group2ID),
               new SqlParameter("@UoM",model.UoM),
               new SqlParameter("@HsnCode",model.HsnCode),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@ItemMake", model.ItemMake.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.ItemGroup.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to InventoryItemGroupModel
        private InventoryItemGroupModel GetModel(IDataRecord dr)
        {
            return new InventoryItemGroupModel
            {
                ItemGroupID = dr.GetInt32("ItemGroupID"),
                ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                Group2ID = dr.GetInt32("Group2ID"),
                UoM = dr.GetString("UoM"),
                HsnCode = dr.GetString("HsnCode"),
                Makes = dr.GetString("Makes"),
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
