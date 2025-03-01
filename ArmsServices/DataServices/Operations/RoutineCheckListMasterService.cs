using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.Operations
{
    public class RoutineCheckListMasterService : IRoutineCheckListMasterService
    {
        IDbService Iservice;

        public RoutineCheckListMasterService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a routine checklist item by its ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.RoutineCheckListMaster.Delete]", parameters);
        }

        // Method to select routine checklist items by branch ID
        public IEnumerable<RoutineCheckListMasterModel> SelectItemByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@ID", BranchID),
            };
            // var cc = Iservice.GetDataReader("[usp.Operation.RoutineCheckListMaster.Select]", parameters);
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.RoutineCheckListMaster.Select]", parameters))
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

        // Method to update a routine checklist item  
        public RoutineCheckListMasterModel Update(RoutineCheckListMasterModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@ItemID", model.ItemID),
               new SqlParameter("@ItemName", model.ItemName),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@IsDisabled", model.IsDisabled),
               new SqlParameter("@ValidDays", model.ValidDays),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.RoutineCheckListMaster.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Helper method to map data record to RoutineCheckListMasterModel
        private RoutineCheckListMasterModel GetModel(IDataRecord dr)
        {
            return new RoutineCheckListMasterModel
            {
                ItemID = dr.GetInt32("ItemID"),
                ItemName = dr.GetString("ItemName"),
                BranchID = dr.GetInt32("BranchID"),
                IsChecked = dr.GetInt32("IsChecked") == 1 ? true : false,
                IsDisabled = dr.GetInt32("IsDisabled") == 1 ? true : false,
                ValidDays = dr.GetInt32("ValidDays"),
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
