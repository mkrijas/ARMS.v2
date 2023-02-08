using ArmsModels.BaseModels.FMS;
using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.FMS
{
    public interface IRoutineCheckListService
    {
        RoutineCheckListModel Update(RoutineCheckListModel model);
        IEnumerable<RoutineCheckListModel> SelectItemByBranch(int? Branch);
        //IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID);
        //AssetDocumentRequestModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);
    }
    public class RoutineCheckListService: IRoutineCheckListService
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
            // var cc = Iservice.GetDataReader("[usp.Operation.RoutineCheckListMaster.Select]", parameters);
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RoutineCheckList.Select]", parameters))
            {
                yield return GetModel(dr);
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
               new SqlParameter("@ItemIDs", model.ItemIDs),
               new SqlParameter("@Description", model.Description),
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
                Description = dr.GetString("Description"),
                CreatedDate = dr.GetDateTime("TimeStamp"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
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
