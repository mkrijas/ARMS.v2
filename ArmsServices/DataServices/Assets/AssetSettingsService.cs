using ArmsModels.BaseModels;
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
        IAssetPostingGroupService _asset;

        public AssetSettingsService(IDbService iservice, IAssetPostingGroupService asset)
        {
            Iservice = iservice;
            _asset = asset;
        }

        public IEnumerable<AssetSettingsModel> SelectAssetSettings(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SubClassID", ID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.SettingsMaster.Select]", parameters))
            {
                yield return new AssetSettingsModel()
                {
                    SettingsID = dr.GetInt32("SettingsID"),
                    SubClassID = dr.GetInt32("SubClassID"),
                    SettingsName = dr.GetString("SettingsName"),
                    SettingsDescription = dr.GetString("SettingsDescription"),
                    RecordStatus = dr.GetBoolean("IsSet"),

                };
            }

        }

    }
}


