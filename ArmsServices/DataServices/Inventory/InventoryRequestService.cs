using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ArmsServices.DataServices.Inventory
{
    public class InventoryRequestService : IInventoryRequestService
    {
        IDbService Iservice;

        public InventoryRequestService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Request.Delete]", parameters);
        }

        public IEnumerable<InventoryItemEntryModel> GetSub(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSub"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Select]", parameters))
            {
                yield return new InventoryItemEntryModel()
                {
                    ItemEntryID = dr.GetInt32("ItemEntryID"),
                    //RID = dr.GetInt32("RID"),
                    ItemID = dr.GetInt32("ItemID"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                };
            }
        }

        public IEnumerable<InventoryRequestModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryRequestModel> SelectByTruckID(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByTruckID"),
               new SqlParameter("@ID", TruckID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryRequestModel> SelectRequestReleaseByTruckID(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByTruckID"),
               new SqlParameter("@ID", TruckID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Release.Select]", parameters))
            {
                yield return GetModelRequestRelease(dr);
            }
        }

        public InventoryRequestModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryRequestModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<InventoryRequestModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryRequestModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InventoryRequestModel Update(InventoryRequestModel model)
        {
            List<InventoryItemEntryModel> ItemsListFormated = new();
            foreach (var item in model.Items)
            {
                ItemsListFormated.Add(new()
                {
                    ItemEntryID = item.ItemEntryID,
                    ItemID = item.ItemID,
                    ItemRate = item.ItemRate,
                    ItemQty = item.ItemQty
                });
            }
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RequestID", model.RequestID),
               new SqlParameter("@RequestDate", model.RequestDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@StoreID", model.Store.StoreID),
               new SqlParameter("@JobcardID", model.Jobcard?.JobcardID??0),
               new SqlParameter("@TruckID", model.Truck?.TruckID??0),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@Items", ItemsListFormated?.ToDataTable()??null),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Request.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private InventoryRequestModel GetModel(IDataRecord dr)
        {
            return new InventoryRequestModel
            {
                RequestID = dr.GetInt32("RequestID"),
                RequestDate = dr.GetDateTime("RequestDate"),
                RequestNumber = dr.GetString("RequestNumber"),
                Store = new()
                {
                    StoreID = dr.GetInt32("StoreID"),
                    StoreName = dr.GetString("StoreName")
                },
                Jobcard = new()
                {
                    JobcardID = dr.GetInt32("JobcardID"),
                    JobcardNumber = dr.GetString("JobcardPrefix") + (dr.GetInt32("JobcardNumber")).ToString()
                },
                Truck = new()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo")
                },
                Remarks = dr.GetString("Remarks"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private InventoryRequestModel GetModelRequestRelease(IDataRecord dr)
        {
            return new InventoryRequestModel
            {
                RequestID = dr.GetInt32("RequestID"),
                RID = dr.GetInt32("RID"),
                RequestDate = dr.GetDateTime("RequestDate"),
                RequestNumber = dr.GetString("RequestNumber"),
                Store = new()
                {
                    StoreID = dr.GetInt32("StoreID"),
                    StoreName = dr.GetString("StoreName")
                },
                Jobcard = new()
                {
                    JobcardID = dr.GetInt32("JobcardID"),
                    JobcardNumber = dr.GetString("JobcardPrefix") + (dr.GetInt32("JobcardNumber")).ToString()
                },
                Truck = new()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo")
                },
                ReleaseSubDetails = new InventoryReleaseSubViewModel()
                {
                    ItemID = dr.GetInt32("ItemID"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    RequestQty = dr.GetDecimal("RequestQty"),
                    ItemQty = dr.GetDecimal("ReleaseQty"),
                },
                Remarks = dr.GetString("Remarks"),
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
