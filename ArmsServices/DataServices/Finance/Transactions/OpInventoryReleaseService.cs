using ArmsModels.BaseModels.General;
using ArmsServices.DataServices.General;
using ArmsServices;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.IDataServices.Finance.Transactions;
using ArmsModels.BaseModels;
using System.Linq;

namespace DAL.DataServices.Finance.Transactions
{
    public class OpInventoryReleaseService : IOpInventoryReleaseService
    {
        IDbService Iservice;
        public OpInventoryReleaseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpInventoryReleaseID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.OpInventoryRelease.Delete]", parameters);

        }

        public IEnumerable<OpInventoryReleaseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.OpInventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OpInventoryReleaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.OpInventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OpInventoryReleaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.OpInventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public OpInventoryReleaseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpInventoryReleaseID", ID),
               new SqlParameter("@Operation", "GetEntries")
            };
            OpInventoryReleaseModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.OpInventoryRelease.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Approve(int? OpInventoryReleaseID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpInventoryReleaseID", OpInventoryReleaseID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Narration", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.OpInventoryRelease.Approve]", parameters);
        }


        public IEnumerable<OpInventoryReleaseSubModel> GetSub(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSub"),
               new SqlParameter("@OpInventoryReleaseID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.OpInventoryRelease.Select]", parameters))
            {
                yield return new OpInventoryReleaseSubModel()
                {
                    OpInventoryReleaseSubID = dr.GetInt32("OpInventoryReleaseSubID"),
                    TruckID = dr.GetInt32("TruckID"),
                    TruckRegNo = dr.GetString("RegNo"),
                    TripID = dr.GetInt64("TripID"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    TripNo = dr.GetString("TripNo"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID")
                };
            }
        }
        public int Reverse(int? OpInventoryReleaseID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpInventoryReleaseID", OpInventoryReleaseID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Narration", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.OpInventoryRelease.Reverse]", parameters);
        }
        public OpInventoryReleaseModel Update(OpInventoryReleaseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpInventoryReleaseID", model.OpInventoryReleaseID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@ItemID", model.Item?.InventoryItemID??0),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@Items", model.Items.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.OpInventoryRelease.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private OpInventoryReleaseModel GetModel(IDataRecord dr)
        {
            return new OpInventoryReleaseModel
            {
                OpInventoryReleaseID = dr.GetInt32("OpInventoryReleaseID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                Item = new InventoryItemModel() { 
                    InventoryItemID = dr.GetInt32("InventoryItemID"),
                 ItemDescription = dr.GetString("ItemDescription"),
                 InventoryItemCode = dr.GetString("InventoryItemCode"),
                },
                Reference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),

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
