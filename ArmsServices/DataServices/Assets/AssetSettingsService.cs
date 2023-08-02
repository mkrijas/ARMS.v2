using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                new SqlParameter("@SubClassID", SubClassID),
            };
            {
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Settings.SelectByID]", parameters))
                {
                    yield return GetModel(dr);
                }
            }
        }

        public AssetSettingsModel Update(AssetSettingsModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@SettingsID", model.SettingsID),
                new SqlParameter("@SubClassID", model.SubClassID),
                new SqlParameter("@RecordStatus", model.RecordStatus),
                new SqlParameter("@UserID", model.UserInfo.UserID)
            };
            Iservice.ExecuteNonQuery("[dbo].[usp.Asset.Settings.Update]", parameters);
            return model;
        }


        public AssetSettingsModel GetModel(IDataRecord dr)
        {
            return new AssetSettingsModel
            {
                SettingsID = dr.GetInt32("SettingsID"),
                SettingsName = dr.GetString("SettingsName"),
                SettingsDescription = dr.GetString("SettingsDescription"),
                RecordStatus = dr.GetBoolean("IsSet"),
            };
        }

    }
}


