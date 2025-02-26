using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    // Service class for managing asset classes and subclasses
    public class AssetClassService : IAssetClassService
    {
        IDbService Iservice; // Database service for executing queries

        // Constructor that initializes the database service
        public AssetClassService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Deletes an asset class by its ID and records the user who performed the deletion
        public int DeleteClass(int? AssetClassID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetClassID", AssetClassID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.AssetClass.Delete]", parameters);
        }

        // Deletes an asset subclass by its ID and records the user who performed the deletion
        public int DeleteSubClass(int? AssetSubClassID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetSubClassID", AssetSubClassID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.AssetSubClass.Delete]", parameters);
        }

        // Retrieves a list of all asset classes
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

        // Retrieves a list of subclasses associated with a specific asset class IDg
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

        // Retrieves a list of subclasses associated with a specific asset class ID
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

        // Updates an existing asset class and returns the updated model
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

        // Updates an existing asset subclass and returns the updated model
        public AssetSubClassModel UpdateSubClass(AssetSubClassModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.AssetSubClassID),
               new SqlParameter("@AsstSubClassName", model.AssetSubclass),
               new SqlParameter("@AssetSubClassAbbrev", model.AssetSubAbbrev),
               new SqlParameter("@AssetClassID", model.AssetClassID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetSubClass.Update]", parameters))
            {
                return GetAssetSubClass(dr);
            }
            return null;
        }

        // Helper method to map data from the database to an AssetClassModel
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

        // Helper method to map data from the database to an AssetSubClassModel
        private AssetSubClassModel GetAssetSubClass(IDataRecord dr)
        {
            return new AssetSubClassModel
            {
                ID = dr.GetInt32("ID"),                
                AssetSubClassID = dr.GetInt32("AssetSubClassID"),                
                AssetSubclass = dr.GetString("AsstSubClassName"),
                AssetSubAbbrev = dr.GetString("AssetSubClassAbbrev"),
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