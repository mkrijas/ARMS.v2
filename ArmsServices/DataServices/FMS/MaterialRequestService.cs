//using ArmsModels.BaseModels;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ArmsServices.DataServices
//{
//    public class MaterialRequestService
//    {
//        IDbService Iservice;

//        public MaterialRequestService(IDbService iservice)
//        {
//            Iservice = iservice;
//        }

//        public int Delete(int? MaterialRequestID, string UserID)
//        {
//            List<SqlParameter> parameters = new List<SqlParameter>
//            {
//               new SqlParameter("@MrID", MaterialRequestID),
//               new SqlParameter("@UserID", UserID),
//            };
//            return Iservice.ExecuteNonQuery("[usp.FMS.MaterialRequest.Delete]", parameters);
//        }

//        public IEnumerable<MaterialRequestModel> Select(int? MaterialRequestID)
//        {
//            List<SqlParameter> parameters = new List<SqlParameter>
//            {
//               new SqlParameter("@MrID", MaterialRequestID)
//            };

//            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.MaterialRequest.Select]", parameters))
//            {
//                yield return GetModel(dr);
//            }
//        }

//        public MaterialRequestModel SelectByID(int? ID)
//        {
//            List<SqlParameter> parameters = new List<SqlParameter>
//            {
//               new SqlParameter("@MrID", ID),
//            };
//            MaterialRequestModel model = new();
//            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.MaterialRequest.Select]", parameters))
//            {
//                model = GetModel(dr);
//            }
//            return model;
//        }

//        public MaterialRequestModel Update(MaterialRequestModel model)
//        {
//            List<SqlParameter> parameters = new List<SqlParameter>
//            {
//               new SqlParameter("@MrID", model.MrID),
//               new SqlParameter("@BranchID", model.BranchID),
//               new SqlParameter("@JobcardID", model.JobcardID),
//               new SqlParameter("@MrDate", model.MrDate),
//               new SqlParameter("@MrNumber", model.MrNumber),
//               new SqlParameter("@StoreID", model.StoreID),
//               new SqlParameter("@UserID", model.UserInfo.UserID),
//            };

//            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.MaterialRequest.Update]", parameters))
//            {
//                model = GetModel(dr);
//            }
//            return model;
//        }

//        private MaterialRequestModel GetModel(IDataRecord dr)
//        {
//            return new MaterialRequestModel
//            {
//                MrID = dr.GetInt32("MrID"),
//                BranchID = dr.GetInt32("BranchID"),
//                JobcardID = dr.GetInt32("JobcardID"),
//                MrDate = dr.GetDateTime("MrDate"),
//                MrNumber = dr.GetString("MrNumber"),
//                StoreID = dr.GetInt32("StoreID"),
//                UserInfo = new ArmsModels.SharedModels.UserInfoModel
//                {
//                    RecordStatus = dr.GetByte("RecordStatus"),
//                    TimeStampField = dr.GetDateTime("TimeStamp"),
//                    UserID = dr.GetString("UserID"),
//                },
//            };
//        }
//    }
//}