using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InventoryItemService : IInventoryItemService
    {
        IDbService Iservice;
        public InventoryItemService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete an inventory item by its ID
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryItemID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Item.Delete]", parameters);
        }

        // Method to search for inventory items by description
        public IEnumerable<InventoryItemModel> SearchByDescription(string itemDescription)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByDesc"),
               new SqlParameter("@itemDescription",itemDescription),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to search for inventory items by HSN code
        public IEnumerable<InventoryItemModel> SearchByHsn(string HsnCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByHsn"),
               new SqlParameter("@HsnCode",HsnCode),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to search for inventory items by item code
        public IEnumerable<InventoryItemModel> SearchByItemCode(string itemCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByCode"),
               new SqlParameter("@InventoryitemCode",itemCode),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select inventory items by group ID
        public IEnumerable<InventoryItemModel> SelectByGroup(int? GroupID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByGroup"),
               new SqlParameter("@InventoryGroupID",GroupID),
                };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select inventory items by item group ID
        public IEnumerable<InventoryItemModel> SelectByItemGroup(int? ItemGroupID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByItemGroup"),
               new SqlParameter("@InventoryGroupID",ItemGroupID),
                };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select an inventory item by its ID
        public InventoryItemModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryItemID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryItemModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select a list of inventory items by their ID
        public IEnumerable<InventoryItemModel> SelectListByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryItemID", ID),
               new SqlParameter("@Operation", "ByID")
                };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update an inventory item
        public InventoryItemModel Update(InventoryItemModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryGroupID", model.InventoryGroupID),
               new SqlParameter("@ItemDescription",model.ItemDescription),
               new SqlParameter("@InventoryItemID",model.InventoryItemID),
               new SqlParameter("@UoM",model.UoM),
               new SqlParameter("@HsnCode",model.HsnCode),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Group2", model.Group2),
               new SqlParameter("@Make", model.Make)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Item.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to get job card time and KM for a specific item and truck
        public IEnumerable<JobCardTrackModel> GetJobcardTimeAndKM(int? ItemID, int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ItemID", ItemID),
               new SqlParameter("@TruckID", TruckID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.JobcardTimeAndKM.Select]", parameters))
            {
                yield return GetModelJobCard(dr);
            }
        }

        // Helper method to map data record to InventoryItemModel
        private InventoryItemModel GetModel(IDataRecord dr)
        {
            return new InventoryItemModel
            {
                HsnCode = dr.GetString("HsnCode"),
                InventoryGroupID = dr.GetInt32("InventoryGroupID"),
                InventoryItemCode = dr.GetString("InventoryItemCode"),
                InventoryItemID = dr.GetInt32("InventoryItemID"),
                UoM = dr.GetString("UoM"),
                ItemDescription = dr.GetString("ItemDescription"),
                Group2 = dr.GetString("Group2"),
                Make = dr.GetString("Make"),
                PartNumber = dr.GetString("PartNumber"),
                Group = new InventoryGroupModel()
                {
                    MappedConsumptionHead = dr.GetInt32("MappedConsumptionHead"),
                    MappedPurchaseHead = dr.GetInt32("MappedPurchaseHead"),
                    MappedNonInventoryPurchaseHead = dr.GetInt32("MappedNonInventoryPurchaseHead"),
                },
                ItemGroup = new InventoryItemGroupModel()
                {
                    Group2ID = dr.GetInt32("Group2ID"),
                    ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Helper method to map data record to JobCardTrackModel
        private JobCardTrackModel GetModelJobCard(IDataRecord dr)
        {
            return new JobCardTrackModel
            {
                DocumentDate = dr.GetDateTime("DocumentDate"),
                Odometer = dr.GetInt32("Odometer"),
                JobCardID = dr.GetInt32("JobCardID"),
                JobcardNumber = dr.GetInt32("JobcardNumber"),
                
            };
        }

    }
}
