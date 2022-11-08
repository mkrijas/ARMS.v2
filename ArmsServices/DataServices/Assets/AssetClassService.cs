using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IAssetClassService
    {
        AssetClassModel UpdateClass(AssetClassModel model);
        AssetClassModel UpdateSubClass(AssetClassModel model);
        IEnumerable<AssetClassModel> SelectSubClasses(int? ID); 
        int Delete(int? AssetClassID, string UserID);
        IEnumerable<AssetClassModel> Select();
        IEnumerable<AssetClassModel> SelectByGroup(int? ID);
    }

    public class AssetClassService : IAssetClassService
    {
        IDbService Iservice;

        public AssetClassService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? AssetClassID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetClassID", AssetClassID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.AssetClass.Delete]", parameters);
        }

        public IEnumerable<AssetClassModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetClass.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        public IEnumerable<AssetClassModel> SelectByGroup(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByGroup"),
               new SqlParameter("@ID", ID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetClass.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public AssetClassModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetClassID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            AssetClassModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetClass.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public AssetClassModel Update(AssetClassModel model)
        {    
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetClassID", model.AssetClassID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@AssetClassTime", model.AssetClassTime),
               new SqlParameter("@AssetClassType", model.AssetClassType),
               new SqlParameter("@ContactNumber", model.ContactNumber),
               new SqlParameter("@Detail", model.Detail),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetClass.Update]", parameters))
            {
                model =  GetAssetClass(dr);
            }
            return model;
        }

        private AssetClassModel GetAssetClass(IDataRecord dr)
        {
            return new AssetClassModel
            {
                AssetClassID = dr.GetInt32("AssetClassID"),
                AssetClassName = dr.GetString("AssetClassName"),
                ParentID = dr.GetInt32("AssetClassID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private AssetClassModel GetAssetSubClass(IDataRecord dr)
        {
            return new AssetClassModel
            {
                AssetClassID = dr.GetInt32("AssetSubClassID"),
                AssetClassName = dr.GetString("AssetSubClassName"),
                ParentID = dr.GetInt32("AssetClassID"),
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
   

     