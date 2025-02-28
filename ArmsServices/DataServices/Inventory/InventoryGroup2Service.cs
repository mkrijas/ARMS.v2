
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InventoryGroup2Service : IInventoryGroup2Service
    {
        IDbService Iservice;
        public InventoryGroup2Service(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete an inventory group by its ID
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryItemID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Item.Group2.Delete]", parameters);
        }

        // Method to search for inventory groups by name
        public IEnumerable<InventoryGroup2Model> SearchByName(string GroupName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "filter"),
               new SqlParameter("@GroupDescription",GroupName),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Group2.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select an inventory group by its ID
        public InventoryGroup2Model SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Group2ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryGroup2Model model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Group2.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update an inventory group
        public InventoryGroup2Model Update(InventoryGroup2Model model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Group2ID", model.Group2ID),
               new SqlParameter("@GroupDescription",model.GroupDescription),
               new SqlParameter("@Group1ID",model.Group1ID),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Group2.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to InventoryGroup2Model
        private InventoryGroup2Model GetModel(IDataRecord dr)
        {
            return new InventoryGroup2Model
            {
                Group2ID = dr.GetInt32("Group2ID"),
                GroupDescription = dr.GetString("GroupDescription"),
                Group1ID = dr.GetInt32("Group1ID"),
                Group = new InventoryGroupModel()
                {
                    InventoryGroupID = dr.GetInt32("Group1ID"),
                    InventoryGroupName = dr.GetString("InventoryGroupName"),
                    MappedPurchaseHead = dr.GetInt32("MappedPurchaseHead"),
                    MappedConsumptionHead = dr.GetInt32("MappedConsumptionHead"),
                    MappedNonInventoryPurchaseHead = dr.GetInt32("MappedNonInventoryPurchaseHead"),
                },
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