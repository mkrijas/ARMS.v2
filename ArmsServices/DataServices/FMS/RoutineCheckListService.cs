using ArmsModels.BaseModels.FMS;
using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.FMS
{
    public class RoutineCheckListService : IRoutineCheckListService
    {
        IDbService Iservice;

        public RoutineCheckListService(IDbService iservice)
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
            return Iservice.ExecuteNonQuery("[usp.FMS.RoutineCheckList.Delete]", parameters);
        }


        public IEnumerable<RoutineCheckListModel> SelectItemByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@ID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RoutineCheckList.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }



        public IEnumerable<RoutineCheckListMasterModel> ExpireItems(int? BranchID, int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@TruckID", TruckID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RoutineCheckList.ExpiredItems.Select]", parameters))
            {
                yield return new RoutineCheckListMasterModel() {
                    ItemID = dr.GetInt32("ItemID"),
                     ItemName = dr.GetString("ItemName"),
                     ValidDays = dr.GetInt32("ValidDays"),
                     Description = dr.GetString("Description"),
                    CurrentTruckLastUpdatedDate = dr.GetDateTime("CurrentTruckLastUpdatedDate")
                };
            }
        }

        //public AssetDocumentRequestModel SelectDocumentRequest(int? ID)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //        new SqlParameter("@ID", ID),
        //       new SqlParameter("@Operation", "ByID")
        //    };

        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentRequest.Select]", parameters))
        //    {
        //        return GetModel(dr);
        //    }
        //    return null;
        //}



        public RoutineCheckListModel Update(RoutineCheckListModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@RoutineCheckListID", model.RoutineCheckListID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@ItemIDs", model.CheckedItemLists.ToDataTable()),
               new SqlParameter("@Narration", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RoutineCheckList.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }


        private RoutineCheckListModel GetModel(IDataRecord dr)
        {
            return new RoutineCheckListModel
            {
                RoutineCheckListID = dr.GetInt32("RoutineCheckListID"),
                ItemIDs = dr.GetString("ItemIDs"),
                BranchID = dr.GetInt32("BranchID"),
                TruckID = dr.GetInt32("TruckID"),
                Remarks = dr.GetString("Narration"),
                CreatedDate = dr.GetDateTime("TimeStamp"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }


        public IEnumerable<RoutineCheckListModel> GetLastRoutineCheckListDetailsUsingID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RoutineCheckList.SelectByTruckID]", parameters))
            {
                yield return new RoutineCheckListModel
                {
                    RoutineCheckListID = dr.GetInt32("RoutineCheckListID"),
                    ItemIDs = dr.GetString("ItemIDs"),
                    BranchID = dr.GetInt32("BranchID"),
                    TruckID = dr.GetInt32("TruckID"),
                    Remarks = dr.GetString("Narration"),
                    Description = dr.GetString("Description"),
                    ItemID = dr.GetInt32("ItemID"),
                    CreatedDate = dr.GetDateTime("TimeStamp"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserId"),
                    },
                };
            }
        }


        public IEnumerable<RoutineCheckListModel> GetLastRoutineCheckListDetailsUsingTruckId(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByTruck"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RoutineCheckList.SelectByTruckID]", parameters))
            {
                yield return new RoutineCheckListModel
                {
                    RoutineCheckListID = dr.GetInt32("RoutineCheckListID"),
                    ItemIDs = dr.GetString("ItemIDs"),
                    BranchID = dr.GetInt32("BranchID"),
                    TruckID = dr.GetInt32("TruckID"),
                    Remarks = dr.GetString("Narration"),
                    Description = dr.GetString("Description"),
                    ItemID = dr.GetInt32("ItemID"),
                    CreatedDate = dr.GetDateTime("TimeStamp"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserId"),
                    },
                };
            }
        }


        //public IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //       new SqlParameter("@ID", RequestID),
        //       new SqlParameter("@Operation", "GetAssets")
        //    };

        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentRequest.Select]", parameters))
        //    {
        //        yield return new AssetModel()
        //        {
        //            AssetID = dr.GetInt32("AssetID"),
        //            Description = dr.GetString("Description"),
        //            AssetCode = dr.GetString("AssetCode"),
        //        };
        //    }
        //}
    }
}
