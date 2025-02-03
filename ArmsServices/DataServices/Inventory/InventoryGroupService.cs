using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InventoryGroupService : IInventoryGroupService
    {
        IDbService Iservice;
        public InventoryGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryItemID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Item.Group.Delete]", parameters);
        }

        public IEnumerable<InventoryGroupModel> SearchByName(string GroupName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "filter"),
               new SqlParameter("@InventoryGroupName",GroupName),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Group.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InventoryGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryGroupID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryGroupModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Group.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public InventoryGroupModel Update(InventoryGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryGroupID", model.InventoryGroupID),
               new SqlParameter("@InventoryGroupName",model.InventoryGroupName),
               new SqlParameter("@MappedConsumptionHead",model.MappedConsumptionHead),
               new SqlParameter("@MappedPurchaseHead",model.MappedPurchaseHead),
               new SqlParameter("@MappedNonInventoryPurchaseHead",model.MappedNonInventoryPurchaseHead),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Group.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private InventoryGroupModel GetModel(IDataRecord dr)
        {
            return new InventoryGroupModel
            {
                InventoryGroupName = dr.GetString("InventoryGroupName"),
                InventoryGroupID = dr.GetInt32("InventoryGroupID"),
                MappedConsumptionHead = dr.GetInt32("MappedConsumptionHead"),
                MappedNonInventoryPurchaseHead = dr.GetInt32("MappedNonInventoryPurchaseHead"),
                MappedPurchaseHead = dr.GetInt32("MappedPurchaseHead"),
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