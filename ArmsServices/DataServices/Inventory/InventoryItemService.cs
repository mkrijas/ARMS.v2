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
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InventoryItemID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Item.Delete]", parameters);
        }



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
