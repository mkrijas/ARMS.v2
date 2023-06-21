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
        AssetSubClassModel UpdateSubClass(AssetSubClassModel model);
        IEnumerable<AssetSubClassModel> SelectSubClasses(int? ID);
        IEnumerable<AssetSubClassModel> SelectSubClassesByClass(int? ID);
        IEnumerable<AssetClassModel> SelectClasses();
        int DeleteClass(int? AssetClassID, string UserID);
        int DeleteSubClass(int? AssetSubClassID, string UserID);
    }

    public class AssetClassService : IAssetClassService
    {
        IDbService Iservice;

        public AssetClassService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int DeleteClass(int? AssetClassID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetClassID", AssetClassID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.AssetClass.Delete]", parameters);
        }
        public int DeleteSubClass(int? AssetSubClassID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetSubClassID", AssetSubClassID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.AssetSubClass.Delete]", parameters);
        }

        public IEnumerable<AssetClassModel> SelectClasses()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetClass.Select]", parameters))
            {
                yield return GetAssetClass(dr);
            }
        }

        public IEnumerable<AssetSubClassModel> SelectSubClasses(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetSubClass.Select]", parameters))
            {
                yield return GetAssetSubClass(dr);
            }
            
        }
        public IEnumerable<AssetSubClassModel> SelectSubClassesByClass(int? ClassID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ClassID),
               new SqlParameter("@Operation", "SelectByClass")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetSubClass.Select]", parameters))
            {
                yield return GetAssetSubClass(dr);
            }

        }

        public AssetClassModel UpdateClass(AssetClassModel model)
        {    
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetClassID", model.AssetClassID),
               new SqlParameter("@AssetClasssName", model.AssetClassName),
                new SqlParameter("@PostingGroupID", model.PostingGroupID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetClass.Update]", parameters))
            {
                return GetAssetClass(dr);
            }
            return null;
        }

        public AssetSubClassModel UpdateSubClass(AssetSubClassModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.AssetSubClassID),
               new SqlParameter("@AsstSubClassName", model.AssetSubclass),
               new SqlParameter("@AssetClassID", model.AssetClassID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetSubClass.Update]", parameters))
            {
                return GetAssetSubClass(dr);
            }
            return null;
        }

        private AssetClassModel GetAssetClass(IDataRecord dr)
        {
            return new AssetClassModel
            {
                AssetClassID = dr.GetInt32("AssetClassID"),
                AssetClassName = dr.GetString("AssetClasssName"),                
                PostingGroupID = dr.GetInt32("PostingGroupID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }

        private AssetSubClassModel GetAssetSubClass(IDataRecord dr)
        {
            return new AssetSubClassModel
            {
                AssetSubClassID = dr.GetInt32("ID"),                
                AssetSubclass = dr.GetString("AsstSubClassName"),
                AssetClassID = dr.GetInt32("AssetClassID"),
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
   

     