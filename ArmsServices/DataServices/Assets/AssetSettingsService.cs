using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class AssetSettingsService : IAssetSettingsService
    {
        IDbService Iservice;

        public AssetSettingsService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<AssetSettingsModel> SelectByID(int? SubClassID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "All"),
                new SqlParameter("@SubClassID", SubClassID),
            };
            {
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Settings.SelectByID]", parameters))
                {
                    yield return GetModel(dr);
                }
            }
        }

        public IEnumerable<AssetSettingsModel> GetSettings(int? SubClassID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "BySubClass"),
                new SqlParameter("@SubClassID", SubClassID),
            };
            {
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Settings.SelectByID]", parameters))
                {
                    yield return GetModel(dr);
                }
            }
        }

        private AssetSettingsModel GetModel(IDataRecord dr)
        {
            return new AssetSettingsModel
            {
                SettingsID = dr.GetInt32("SettingsID"),
                SubClassID = dr.GetInt32("SubClassID"),
                SettingsName = dr.GetString("SettingsName"),
                SettingsDescription = dr.GetString("SettingsDescription"),
                IsActive = dr.GetBoolean("IsSet"),
            };
        }

        //public AssetSettingsModel Update(AssetSettingsModel model)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //        new SqlParameter("@SettingsID", model.SettingsID),
        //        new SqlParameter("@SubClassID", model.SubClassID),
        //        new SqlParameter("@RecordStatus", model.RecordStatus),
        //        new SqlParameter("@UserID", model.UserInfo.UserID)
        //    };
        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Settings.Update]", parameters))
        //    {
        //        model = new AssetSettingsModel
        //        {
        //            SettingsID = dr.GetInt32("SettingsID"),
        //            SubClassID = dr.GetInt32("SubClassID"),
        //            RecordStatus = dr.GetBoolean("RecordStatus"),
        //            UserInfo = new ArmsModels.SharedModels.UserInfoModel
        //            {
        //                UserID = dr.GetString("UserID"),
        //            },
        //        };
        //    }
        //    return model;
        //}

        public int Update(AssetSettingsModel obj, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SettingsID", obj.SettingsID),
               new SqlParameter("@SubClassID", obj.SubClassID),
               new SqlParameter("@IsActive", obj.IsActive),
               new SqlParameter("@UserID", UserID)
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Settings.Update]", parameters);
        }


    }

}


